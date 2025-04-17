using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Medpharm.Services.IService;

namespace Medpharm.Services
{
    public class EmailService : IEmailService
    {
        private readonly string smtpServer = "smtp.sendgrid.net";
        private readonly int smtpPort = 587; // Use 465 for SSL
        private readonly string smtpUsername = "apikey"; // Always use 'apikey' for SendGrid
        private readonly string smtpPassword = "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";
        private readonly string fromEmail = "meetd078@gmail.com";
        private readonly string fromName = "Medpharm Support";

        public async Task SendEmailAsync(string toEmail, string subject, string plainTextContent, string htmlContent)
        {
            var smtpClient = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = subject,
                Body = plainTextContent,
                IsBodyHtml = false
            };

            mailMessage.To.Add(toEmail);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
                throw;
            }
        }
    }
}