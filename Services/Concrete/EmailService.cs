﻿using EduSat.TestSeries.Service.Services.Interfaces;
using System.Net.Mail;
using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using EduSat.TestSeries.Service.Domain.Models.Notification;
using EduSat.TestSeries.Service.Models.DTOs.Request.Notification;
using Newtonsoft.Json.Linq;
using System.Text;
using EduSat.TestSeries.Service.Models.DTOs.Response;

namespace EduSat.TestSeries.Service.Services.Concrete
{
    public class EmailService: IMessageService
    {
        private readonly EmailConfig _config;
        public EmailService()
        {
            _config = new EmailConfig
            {
                SmtpUsername = "testseriesedusat@gmail.com",
                SmtpPassword = "etcb oyor fxpz aybo",
                FromName = "Edusat Test Series",
                FromAddress = "testseriesedusat@gmail.com"
            };

        }
        public async Task<bool> sendMessage(NotificationRequest notificationRequest, SchoolDetails recipient)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_config.FromName, _config.FromAddress));
                message.To.Add(new MailboxAddress("", recipient.TeacherEmail));
                message.Subject = notificationRequest.Subject;

                var body = new TextPart(TextFormat.Plain)
                {
                    Text = notificationRequest.Body
                };
                
                if (notificationRequest.Attachment != null)
                {
                    byte[] fileBytes;
                    using (var stream = new MemoryStream())
                    {
                        await notificationRequest.Attachment.CopyToAsync(stream);
                        fileBytes = stream.ToArray();
                    }

                    var attachment = new MimePart(notificationRequest.Attachment.ContentType)
                    {
                        Content = new MimeContent(new MemoryStream(fileBytes), ContentEncoding.Default),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = Path.GetFileName(notificationRequest.Attachment.FileName)
                    };

                    var multipart = new Multipart("mixed");
                    multipart.Add(body);
                    multipart.Add(attachment);

                    message.Body = multipart;
                }
                else
                {
                    message.Body = body;
                }


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
