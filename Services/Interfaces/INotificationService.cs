using EduSat.TestSeries.Service.Models.DTOs.Request.Notification;

namespace EduSat.TestSeries.Service.Services.Interfaces
{
    public interface INotificationService
    {
        Task<bool> Notify(NotificationRequest notificationRequest);
    }
}
