namespace EduSat.TestSeries.Service.Models.DTOs.Request.Notification
{
    public class NotificationRequest
    {
        public MessageDetails[] MessageDetails { get; set; } = [];
        public string Mode { get; set; } = string.Empty;

    }
}
