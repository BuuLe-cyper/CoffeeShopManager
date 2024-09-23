using BussinessObjects.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace BusinessObjects.Services
{
    public class MailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = false)
        {
            try
            {
                using (var client = new SmtpClient(_mailSettings.SmtpServer, _mailSettings.SmtpPort))
                {
                    client.Credentials = new NetworkCredential(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                    client.EnableSsl = true; // Set to true if using an SSL connection

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_mailSettings.SmtpUser),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = isHtml
                    };

                    mailMessage.To.Add(toEmail);

                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
