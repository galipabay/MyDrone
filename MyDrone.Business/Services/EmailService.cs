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
            try
            {
                var smtpClient = new SmtpClient(_smtpHost)
                {
                    Port = _smtpPort,
                    Credentials = new NetworkCredential(_username, _password),
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
            catch (Exception ex)
            {
                // Hata mesajını loglayabiliriz
                Console.WriteLine($"E-posta gönderme hatası: {ex.Message}");
                throw;
            }
        }
    }
}
