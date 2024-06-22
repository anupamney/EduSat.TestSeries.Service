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
        public TagDetailsService(ITagDetailsProvider tagDetailsProvider)
        {
            _provider = tagDetailsProvider;
        }
        
        public string GetInvoiceLink()
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
                    return linkToFile.ToString();
                }
            }
            else
            {
                Console.WriteLine("Templates folder does not exist.");
            }

            return "";
        }

        public string GetReceiptLink()
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
                    return linkToFile.ToString();
                }
            }
            else
            {
                Console.WriteLine("Templates folder does not exist.");
            }

            return "";
        }
    }
}
