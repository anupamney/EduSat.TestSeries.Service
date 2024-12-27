using EduSat.TestSeries.Service.Models.DTOs.Response;
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
        private readonly ITagService _tagService;
        public TagDetailsService(ITagDetailsProvider tagDetailsProvider, ITagService tagService)
        {
            _provider = tagDetailsProvider;
            _tagService = tagService;
        }
        
        //public string GetInvoiceLink(SchoolDetails recipient)
        //{
        //    string currentDirectory = Directory.GetCurrentDirectory();
        //    // Combine the current directory path with the "templates" folder path
        //    string templatesFolderPath = Path.Combine(currentDirectory, "Templates");

        //    // Check if the directory exists
        //    if (Directory.Exists(templatesFolderPath))
        //    {
        //        using (WebClient client = new WebClient())
        //        {
        //            var resStr = client.UploadFile("https://file.io", $"{templatesFolderPath}/invoice.html");
        //            var jObjResult = JObject.Parse(Encoding.UTF8.GetString(resStr));
        //            var linkToFile = jObjResult["link"];
        //            return linkToFile.ToString();
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Templates folder does not exist.");
        //    }

        //    return "";
        //}
        public async Task<string> GetInvoiceLinkAsync(SchoolDetails recipient)
        {
            string invoiceTemplate = GetTemplateContent("invoice.html");
            invoiceTemplate = await _tagService.ResolveTags(invoiceTemplate, recipient);
            return UploadFile(invoiceTemplate);
        }

        public async Task<string> GetReceiptLinkAsync(SchoolDetails recipient)
        {
            string receiptTemplate = GetTemplateContent("receipt.html");
            receiptTemplate = await _tagService.ResolveTags(receiptTemplate, recipient);
            return UploadFile(receiptTemplate);
        }
        private string UploadFile(string content)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    var resStr = client.UploadFile("https://file.io", content);
                    var jObjResult = JObject.Parse(Encoding.UTF8.GetString(resStr));
                    var linkToFile = jObjResult["link"];
                    return linkToFile.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return String.Empty;
        }
        private string GetTemplateContent(string templateFileName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine(currentDirectory);
            string templatesFolderPath = Path.Combine(currentDirectory, "Templates");

            if (Directory.Exists(templatesFolderPath))
            {
                string templatePath = Path.Combine(templatesFolderPath, templateFileName);
                if (File.Exists(templatePath))
                {
                    return File.ReadAllText(templatePath);
                }
                else
                {
                    Console.WriteLine($"{templateFileName} template does not exist.");
                }
            }
            else
            {
                Console.WriteLine("Templates folder does not exist.");
            }

            return string.Empty;
        }
    }
}
