using EduSat.TestSeries.Service.Models.DTOs.Request.Notification;
using EduSat.TestSeries.Service.Services.Interfaces;
using System.Net.Mail;
using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace EduSat.TestSeries.Service.Services.Concrete
{
    public class EmailService: IMessageService
    {
        private readonly EmailConfig _config;
        public EmailService()
        {
            _config = new EmailConfig
            {
                SmtpUsername = "anupamshandilya28@gmail.com",
                SmtpPassword = "rzrz tqss jhuh risy",
                FromName = "Your Name",
                FromAddress = "anupamshandilya28@gmail.com"
            };

        }
        public async Task<bool> sendMessage(MessageDetails messageDetails)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_config.FromName, _config.FromAddress));
                message.To.Add(new MailboxAddress("", messageDetails.Recipient));
                message.Subject = messageDetails.Subject;
                message.Body = new TextPart(TextFormat.Plain)
                {
                    Text = messageDetails.Body
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync(_config.SmtpServer, _config.SmtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_config.SmtpUsername, _config.SmtpPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                // Handle exception (log it, rethrow it, etc.)
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
