using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using MyDrone.Kernel.Services;

namespace MyDrone.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _username;
        private readonly string _password;

        public EmailService(IConfiguration configuration)
        {
            // Appsettings.json'dan SMTP ayarlarını alıyoruz
            _smtpHost = configuration["EmailSettings:Host"];
            _smtpPort = int.Parse(configuration["EmailSettings:Port"]);
            _username = configuration["EmailSettings:Username"];
            _password = configuration["EmailSettings:Password"];
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            if (string.IsNullOrEmpty(toEmail))
                throw new ArgumentNullException(nameof(toEmail), "To email cannot be null or empty.");
            if (string.IsNullOrEmpty(subject))
                throw new ArgumentNullException(nameof(subject), "Subject cannot be null or empty.");
            if (string.IsNullOrEmpty(body))
                throw new ArgumentNullException(nameof(body), "Body cannot be null or empty.");

            var smtpClient = new SmtpClient(_smtpHost)
            {
                Port = _smtpPort,
                Credentials = new NetworkCredential(_username, "wvhu bwsh xqyo ghqa"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_username),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);
            smtpClient.Send(mailMessage);
        }


        
    }
}
