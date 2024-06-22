using EduSat.TestSeries.Service.Models.DTOs.Request.Notification;
using EduSat.TestSeries.Service.Models.DTOs.Response;
using EduSat.TestSeries.Service.Services.Interfaces;
using System.Text.RegularExpressions;

namespace EduSat.TestSeries.Service.Services.Concrete
{
    public class NotificationService: INotificationService
    {
        private readonly IEnumerable<IMessageService> _services;
        private readonly ITagDetailsService _tagDetailsService;

        public NotificationService(IEnumerable<IMessageService> services, ITagDetailsService tagDetailsService)
        {
            _services = services;
            _tagDetailsService = tagDetailsService;
        }

        public async Task<bool> Notify(NotificationRequest notificationRequest, SchoolDetails[] recipients)
        {
            var instance = _services.FirstOrDefault(x => x.GetType().Name == notificationRequest.Mode);
            if (instance == null)
            {
                return false;
            }

            var tasks = new List<Task>();
            for (int i= 0; i < recipients.Length;i++)
            {
                notificationRequest.Body = await ResolveTags(notificationRequest.Body, recipients[i]);
                tasks.Add(instance.sendMessage(notificationRequest, recipients[i]));
            }

            await Task.WhenAll(tasks);
            return true;

        }

        private async Task<string> ResolveTags(string body, SchoolDetails recipient)
        {
            List<string> words = ExtractWordsBetweenSymbols(body, '@');

            Dictionary<string, string> replacements = await InitialiseTags(words, recipient);

            string updatedInput = ReplacePlaceholders(body, replacements);

            return updatedInput;
        }
        private List<string> ExtractWordsBetweenSymbols(string input, char symbol)
        {
            List<string> words = new List<string>();
            Regex regex = new Regex($@"{symbol}(.*?){symbol}");
            MatchCollection matches = regex.Matches(input);

            foreach (Match match in matches)
            {
                words.Add(match.Groups[1].Value);
            }

            return words;
        }

        private async Task<Dictionary<string, string>> InitialiseTags(List<string> words,SchoolDetails recipient)
        {
            Dictionary<string, string> tagsMap = new Dictionary<string, string>
        {
            { "FirstName",recipient.TeacherFirstName},
            { "LastName", recipient.TeacherLastName},
            {"RemainingAmount", (recipient.TotalPayment-recipient.TotalPaymentReceived).ToString()},
            {"TotalAmount", recipient.TotalPayment.ToString() },
            {"ReceiptLink", _tagDetailsService.GetReceiptLink() },
            {"InvoiceLink", _tagDetailsService.GetInvoiceLink() }
        };

            Dictionary<string, string> replacements = new Dictionary<string, string>();

            foreach (string word in words)
            {
                if (tagsMap.ContainsKey(word))
                {
                    replacements[$"@{word}@"] = tagsMap[word];
                }
                else
                {
                    replacements[$"@{word}@"] = $"No service found for {word}";
                }
            }

            return replacements;
        }

        static string ReplacePlaceholders(string input, Dictionary<string, string> replacements)
        {
            foreach (var replacement in replacements)
            {
                input = input.Replace(replacement.Key, replacement.Value);
            }

            return input;
        }
    }
}
