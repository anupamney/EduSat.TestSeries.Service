using EduSat.TestSeries.Service.Models.DTOs.Request.Notification;
using EduSat.TestSeries.Service.Services.Interfaces;

namespace EduSat.TestSeries.Service.Services.Concrete
{
    public class NotificationService: INotificationService
    {
        private readonly IEnumerable<IMessageService> _services;

        public NotificationService(IEnumerable<IMessageService> services)
        {
            _services = services;
        }

        public async Task<bool> Notify(NotificationRequest notificationRequest)
        {
            foreach( var x in _services)
            {
                Console.WriteLine(x.GetType().Name);
            }
            var instance = _services.FirstOrDefault(x => x.GetType().Name == notificationRequest.Mode);
            if (instance == null)
            {
                return false;
            }

            var tasks = new List<Task>();
            foreach (var messageDetails in notificationRequest.MessageDetails)
            {
                tasks.Add(instance.sendMessage(messageDetails));
            }

            await Task.WhenAll(tasks);
            return true;

        }
    }
}
