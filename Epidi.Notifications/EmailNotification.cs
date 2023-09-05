using Epidi.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;
using System.Text.RegularExpressions;

namespace Epidi.Notifications
{
    public class EmailNotification : IDisposable
    {
        private EmailConfigs _emailConfigs;
        public EmailNotification(EmailConfigs emailConfigs)
        {
            _emailConfigs = emailConfigs;
        }

        public void SendEmailViaSMTP(string receiverEmail, string subject, string body)
        {
            if (string.IsNullOrEmpty(receiverEmail))
                throw new Exception($"Email can not be null.");
            if (string.IsNullOrEmpty(_emailConfigs.SenderEmail))
                throw new Exception($"Please specify the Sender Email in AppSettings.");
            if (string.IsNullOrEmpty(_emailConfigs.SmtpAddress))
                throw new Exception($"Please specify the SMTP Address in AppSettings.");
            if (string.IsNullOrEmpty(_emailConfigs.Password))
                throw new Exception($"Please specify the Password in AppSettings.");
            if (string.IsNullOrEmpty(_emailConfigs.Port.ToString()))
                throw new Exception($"Please specify the Port in AppSettings.");

            try
            {
                using var mail = new MailMessage
                {
                    From = new MailAddress(_emailConfigs.SenderEmail, _emailConfigs.DisplayName)
                };
                mail.To.Add(receiverEmail);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = body;
                using SmtpClient smtp = new SmtpClient(_emailConfigs.SmtpAddress, _emailConfigs.Port);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_emailConfigs.SenderEmail, _emailConfigs.Password);
                smtp.Host = _emailConfigs.SmtpAddress;
                smtp.Port = _emailConfigs.Port;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> SendEmailViaSendGrid(string receiverEmail, string subject, string body)
        {
            try
            {
                var apiKey = _emailConfigs.Key;
                var client = new SendGridClient(apiKey);

                var from_email = new EmailAddress("ka@epidi.com", "Epidi - No Reply");
                var to_email = new EmailAddress(receiverEmail, receiverEmail);
                string htmlContent = body;

                //if (subject == "Registration")
                //{
                //    htmlContent = GetRegistrationEmailBody(receiverEmail);
                //}
                //else if (subject == "Forgot")
                //{
                //    subject = "Forgot password process";
                //}

                var msg = new SendGridMessage()
                {
                    From = from_email,
                    Subject = subject,
                    PlainTextContent = StripHTML(htmlContent),
                    HtmlContent = htmlContent
                };
                msg.AddTo(to_email);
                var response = await client.SendEmailAsync(msg);

                return response.StatusCode.ToString();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.Message;
            }
        }

        private static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _emailConfigs = null;
            }
        }

    }
}
