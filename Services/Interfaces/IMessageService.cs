using EduSat.TestSeries.Service.Models.DTOs.Request.Notification;

namespace EduSat.TestSeries.Service.Services.Interfaces
{
    public interface IMessageService
    {
        Task<bool> sendMessage(MessageDetails messageDetails);
    }
}
