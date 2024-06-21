using EduSat.TestSeries.Service.Models.DTOs.Request.Notification;
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

        public async Task<bool> Notify(NotificationRequest notificationRequest)
        {
            var instance = _services.FirstOrDefault(x => x.GetType().Name == notificationRequest.Mode);
            if (instance == null)
            {
                return false;
            }

            var tasks = new List<Task>();
            for (int i= 0; i < notificationRequest.Recipients.Length;i++)
            {
                notificationRequest.Body = await ResolveTags(notificationRequest.Body, notificationRequest.srns[i]);
                tasks.Add(instance.sendMessage(notificationRequest, notificationRequest.Recipients[i]));
            }

            await Task.WhenAll(tasks);
            return true;

        }

        private async Task<string> ResolveTags(string body, string srn)
        {
            List<string> words = ExtractWordsBetweenSymbols(body, '@');

            Dictionary<string, string> replacements = await CallServicesDynamicallyAsync(words, srn);

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

        private async Task<Dictionary<string, string>> CallServicesDynamicallyAsync(List<string> words,string srn)
        {
            Dictionary<string, Func<string,Task<string>>> services = new Dictionary<string, Func<string,Task<string>>>
        {
            { "FirstName", _tagDetailsService.GetTeachersFirstName},
            { "LastName", _tagDetailsService.GetTeachersLastName},
            {"RemainingAmount", _tagDetailsService.GetRemainingAmount},
            {"TotalAmount", _tagDetailsService.GetTotalAmount },
            {"ReceiptLink", _tagDetailsService.GetReceiptLink },
            {"InvoiceLink", _tagDetailsService.GetInvoiceLink }
        };

            Dictionary<string, string> replacements = new Dictionary<string, string>();

            foreach (string word in words)
            {
                if (services.ContainsKey(word))
                {
                    var result = await services[word](srn);
                    replacements.Add($"@{word}@", result);
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
