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
        private readonly ITagDetailsService _tagDetailsService;
        private readonly ITagService _tagService;

        public NotificationService(IEnumerable<IMessageService> services, ITagDetailsService tagDetailsService, ITagService tagService)
        {
            _services = services;
            _tagDetailsService = tagDetailsService;
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
            for (int i= 0; i < recipients.Length;i++)
            {
                notificationRequest.Body = await _tagService.ResolveTags(notificationRequest.Body, recipients[i]);

                tasks.Add(instance.sendMessage(notificationRequest, recipients[i]));
            }

            await Task.WhenAll(tasks);
            return true;

        }
        
    }

}
