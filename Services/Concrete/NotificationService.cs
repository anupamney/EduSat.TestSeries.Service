using EduSat.TestSeries.Service.Models.DTOs.Request.Notification;
using EduSat.TestSeries.Service.Models.DTOs.Response;
using EduSat.TestSeries.Service.Services.Interfaces;
using System.Text.RegularExpressions;
using Twilio.Rest.Trunking.V1;

namespace EduSat.TestSeries.Service.Services.Concrete
{
    public class NotificationService: INotificationService
    {
        private readonly IEnumerable<IMessageService> _services;
        private readonly ITagService _tagService;

        public NotificationService(IEnumerable<IMessageService> services, ITagService tagService)
        {
            _services = services;
            _tagService = tagService;
        }

        public async Task<bool> Notify(NotificationRequest notificationRequest, SchoolDetails[] recipients)
        {
            var instance = _services.FirstOrDefault(x => x.GetType().Name == notificationRequest.Mode);
            if (instance == null)
            {
                return false;
            }

            var tasks = new List<Task>();
            var messageTemplate = notificationRequest.Body;
            for (int i= 0; i < recipients.Length;i++)
            {
                var recipient = recipients[i];
                recipient.Invoice = await _tagService.ResolveTags(GetTemplateContent("Invoice.html"), recipient);
                recipient.Receipt = await _tagService.ResolveTags(GetTemplateContent("Receipt.html"), recipient);


                notificationRequest.Body = await _tagService.ResolveTags(messageTemplate, recipient);


                tasks.Add(instance.sendMessage(notificationRequest, recipient));
            }

            await Task.WhenAll(tasks);
            return true;

        }

        private string GetTemplateContent(string templateFileName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string templatesFolderPath = Path.Combine(currentDirectory, "Templates");

            Console.WriteLine($"Current Directory: {currentDirectory}");

            // Get all folders in the current directory
            string[] folders = Directory.GetDirectories(currentDirectory);

            // Print each folder
            Console.WriteLine("Folders:");
            foreach (string folder in folders)
            {
                Console.WriteLine(folder);
            }

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
