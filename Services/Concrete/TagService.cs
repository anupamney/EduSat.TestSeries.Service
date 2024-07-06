using EduSat.TestSeries.Service.Models.DTOs.Response;
using EduSat.TestSeries.Service.Provider;
using EduSat.TestSeries.Service.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace EduSat.TestSeries.Service.Services.Concrete
{
    public class TagService : ITagService
    {
        private readonly ISchoolsProvider _schoolProvider;
        public TagService(ISchoolsProvider schoolsProvider)
        {
            _schoolProvider = schoolsProvider;
        }
        public async Task<string> ResolveTags(string body, SchoolDetails recipient)
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

        private async Task<Dictionary<string, string>> InitialiseTags(List<string> words, SchoolDetails recipient)
        {
            DateTime date = DateTime.Now;
            var (addressOne, addressTwo) = await _schoolProvider.GetAddress(recipient.Id);
            Dictionary<string, string> tagsMap = new Dictionary<string, string>
            {
                { "FirstName",recipient.TeacherFirstName},
                { "LastName", recipient.TeacherLastName},
                {"RemainingAmount", (recipient.TotalPayment-recipient.TotalPaymentReceived).ToString()},
                {"TotalAmount", recipient.TotalPayment.ToString() },
                {"ReceiptLink", UploadFile(recipient.Receipt)},
                {"InvoiceLink", UploadFile(recipient.Invoice)},
                {"serialNo", recipient.Id.ToString()+"_"+ Guid.NewGuid().ToString().Substring(0,20) },
                {"date",  date.Day.ToString()},
                {"month", date.Month.ToString()},
                {"year", date.Year.ToString() },
                {"Addressline1", addressOne},
                {"Addressline2", addressTwo },
                {"counter", "1" },
                {"classname", recipient.ClassName},
                {"numberOfStudents", recipient.TotalStudents.ToString()},
                {"rate", (recipient.TotalPayment/recipient.TotalStudents).ToString("F2")},
                {"amountPaid", recipient.TotalPaymentReceived.ToString()},
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
        private string UploadFile(string content)
        {
            if(content == null)
                return String.Empty;
            string filePath = $"./receipt{Guid.NewGuid()}.html";

            try
            {
                File.WriteAllText(filePath, content);
            }
            catch (Exception ex)
            {
                // Handle any errors that might occur
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            try
            {
                using (WebClient client = new WebClient())
                {
                    var resStr = client.UploadFile("https://file.io", filePath);
                    var jObjResult = JObject.Parse(Encoding.UTF8.GetString(resStr));
                    var linkToFile = jObjResult["link"];
                    File.Delete(filePath);
                    return linkToFile.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return String.Empty;
        }
    }
}
