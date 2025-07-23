using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace E_Commerce.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;    
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpConfig = _config.GetSection("Smtp").Get<SmtpConfig>();
            var client = new SmtpClient(smtpConfig.Host, smtpConfig.Port)
            {
                Credentials = new NetworkCredential(smtpConfig.Username, smtpConfig.Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpConfig.Username),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);

        }
       
    }
}
