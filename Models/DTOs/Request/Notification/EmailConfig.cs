namespace EduSat.TestSeries.Service.Models.DTOs.Request.Notification
{
    public class EmailConfig
    {
        public string SmtpServer { get; set; } = "smtp.gmail.com";
        public int SmtpPort { get; set; } = 587; // Use 465 for SSL
        public string SmtpUsername { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public string FromAddress { get; set; } = string.Empty;
    }
}
