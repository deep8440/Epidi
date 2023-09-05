using Epidi.Models.Model;
using Epidi.Notifications.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Notifications
{
    public class Messages : IMessages
    {
        private EmailConfigs EmailConfig { get; set; }

        public Messages(IOptions<EmailConfigs> EmailConfigOption)
        {
            this.EmailConfig = EmailConfigOption.Value;
        }
        public async Task<string> SendEmailViaSendGrid(string receiverEmail, string subject, string body)
        {
            using EmailNotification smtp = new EmailNotification(EmailConfig);
            return await smtp.SendEmailViaSendGrid(receiverEmail, subject, body);
        }

        public void SendEmailViaSMTP(string receiverEmail, string subject, string body)
        {
            using EmailNotification smtp = new EmailNotification(EmailConfig);
            smtp.SendEmailViaSMTP(receiverEmail, subject, body);
        }
    }
}
