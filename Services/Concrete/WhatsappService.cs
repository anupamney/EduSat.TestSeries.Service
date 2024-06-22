using Edusat.TestSeries.Service.Domain.Models.Notification;
using EduSat.TestSeries.Service.Models.DTOs.Request.Notification;
using EduSat.TestSeries.Service.Models.DTOs.Response;
using EduSat.TestSeries.Service.Services.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Trunking.V1;
using Twilio.Types;

namespace EduSat.TestSeries.Service.Services.Concrete
{
    public class WhatsappService : IMessageService
    {
        private readonly WhatsappConfig _whatsappConfig;
        public WhatsappService()
        {
            _whatsappConfig = new WhatsappConfig
            {
                TwilioAccountSid = "AC6942c6b62c6af98b2102302053bf5c80",
                TwilioAuthToken = "acd9558ed625fc7b3250c3f47b6bcc86",
                TwilioPhoneNumber = new PhoneNumber("whatsapp:+14155238886")
            };
        }
        public Task<bool> sendMessage(NotificationRequest messageDetails, SchoolDetails recipient)
        {
            TwilioClient.Init(_whatsappConfig.TwilioAccountSid, _whatsappConfig.TwilioAuthToken);
            try
            {
                // Send the message
                var message = MessageResource.Create(
                    body: "Here is an image for you!",
                    from: _whatsappConfig.TwilioPhoneNumber,
                    to: new PhoneNumber($"whatsapp:+91{recipient.TeacherContact}")
                );

                // Output the message SID to the console
                Console.WriteLine($"Message SID: {message.Sid}");
            }
            catch (Exception ex)
            {
                // Handle any errors that occur
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return Task.FromResult(true);
        }
    }
}
