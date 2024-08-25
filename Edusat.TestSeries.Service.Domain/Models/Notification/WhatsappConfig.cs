using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Types;

namespace Edusat.TestSeries.Service.Domain.Models.Notification
{
    public class WhatsappConfig
    {
        public string TwilioAccountSid { get; set; } = string.Empty;
        public string TwilioAuthToken { get; set; } = string.Empty;
        public PhoneNumber TwilioPhoneNumber { get; set; } = string.Empty;
    }
}
