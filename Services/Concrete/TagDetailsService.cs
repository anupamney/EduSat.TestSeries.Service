using EduSat.TestSeries.Service.Provider;
using EduSat.TestSeries.Service.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace EduSat.TestSeries.Service.Services.Concrete
{
    public class TagDetailsService : ITagDetailsService
    {
        private readonly ITagDetailsProvider _provider;
        private int sdid=0;
        private int teacherId=0;
        private string firstName;
        private string lastName;
        private string totalAmount;
        public TagDetailsService(ITagDetailsProvider tagDetailsProvider)
        {
            _provider = tagDetailsProvider;
        }
        private async Task GetsdAsync()
        {
            var sd= await _provider.GetSchoolDetails(sdid);
            sdid = sd.Item1;
            teacherId = sd.Item2;
        }
        private async Task GetTeacherAsync()
        {
            var teacher = await _provider.GetTeacher(teacherId);
            firstName = teacher.FirstName;
            lastName = teacher.LastName;
        }
        public Task<string> GetInvoiceLink(string srn)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            // Combine the current directory path with the "templates" folder path
            string templatesFolderPath = Path.Combine(currentDirectory, "Templates");

            // Check if the directory exists
            if (Directory.Exists(templatesFolderPath))
            {
                using (WebClient client = new WebClient())
                {
                    var resStr = client.UploadFile("https://file.io", $"{templatesFolderPath}/invoice.html");
                    var jObjResult = JObject.Parse(Encoding.UTF8.GetString(resStr));
                    var linkToFile = jObjResult["link"];
                    return Task.FromResult(linkToFile.ToString());
                }
            }
            else
            {
                Console.WriteLine("Templates folder does not exist.");
            }

            return Task.FromResult("");
        }

        public Task<string> GetReceiptLink(string srn)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            // Combine the current directory path with the "templates" folder path
            string templatesFolderPath = Path.Combine(currentDirectory, "Templates");

            // Check if the directory exists
            if (Directory.Exists(templatesFolderPath))
            {
                using (WebClient client = new WebClient())
                {
                    var resStr = client.UploadFile("https://file.io", $"{templatesFolderPath}/receipt.html");
                    var jObjResult = JObject.Parse(Encoding.UTF8.GetString(resStr));
                    var linkToFile = jObjResult["link"];
                    return Task.FromResult(linkToFile.ToString());
                }
            }
            else
            {
                Console.WriteLine("Templates folder does not exist.");
            }

            return Task.FromResult("");
        }

        public async Task<string> GetRemainingAmount(string srn)
        {
            if(teacherId == 0 || sdid == 0)
            {
                await GetsdAsync(srn);
            }
            var remainingAmount = await _provider.GetRemainingAmount(sdid);

            return remainingAmount.Item1.ToString();
        }

        public async Task<string> GetTeachersFirstName(string srn)
        {
            if(teacherId == 0 || sdid == 0)
            {
                await GetsdAsync();
            }
            if(firstName == null)
            {
                await GetTeacherAsync();
            }

            return firstName;
        }

        public async Task<string> GetTeachersLastName(string srn)
        {
            if(teacherId == 0 || sdid == 0)
            {
                await GetsdAsync();
            }
            if(lastName == null)
            {
                await GetTeacherAsync();
            }

            return lastName;
        }

        public async Task<string> GetTotalAmount(string srn)
        {
            var tp = await _provider.GetRemainingAmount(sdid);
            
            return tp.Item2.ToString();
        }
    }
}
