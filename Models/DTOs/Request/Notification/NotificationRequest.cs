using EduSat.TestSeries.Service.Models.DTOs.Response;

namespace EduSat.TestSeries.Service.Models.DTOs.Request.Notification
{
    public class NotificationRequest
    {
        public string Recipients { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty ;
        public IFormFile? Attachment { get; set; }
        public string Mode { get; set; } = string.Empty;

    }
}
