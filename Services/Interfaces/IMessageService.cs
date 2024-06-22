using EduSat.TestSeries.Service.Models.DTOs.Request.Notification;
using EduSat.TestSeries.Service.Models.DTOs.Response;

namespace EduSat.TestSeries.Service.Services.Interfaces
{
    public interface IMessageService
    {
        Task<bool> sendMessage(NotificationRequest messageDetails,SchoolDetails contact);
    }
}
