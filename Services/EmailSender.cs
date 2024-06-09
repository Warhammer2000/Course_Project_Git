using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CourseProjectItems.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CourseProjectItems.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpSettings = _configuration.GetSection("Authentication:Smtp");

            var host = smtpSettings["Host"];
            var portString = smtpSettings["Port"];
            var enableSslString = smtpSettings["EnableSsl"];
            var userName = smtpSettings["UserName"];
            var password = smtpSettings["Password"];


            if (host == null || portString == null || enableSslString == null || userName == null || password == null)
            {
                throw new ArgumentNullException("One or more SMTP settings are null");
            }

            var port = int.Parse(portString);
            var enableSsl = bool.Parse(enableSslString);

            var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(userName, password),
                EnableSsl = enableSsl
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(userName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }

    }
}
