using EduSat.TestSeries.Service.Models.DTOs.Request.Notification;
using EduSat.TestSeries.Service.Models.DTOs.Response;

namespace EduSat.TestSeries.Service.Services.Interfaces
{
    public interface INotificationService
    {
        Task<bool> Notify(NotificationRequest notificationRequest, SchoolDetails[] recipients);
    }
}
