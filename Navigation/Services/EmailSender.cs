using System;
using System.Buffers.Text;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace Navigation.Services
{
    public class EmailSender: IEmailSender
    {
        public IConfiguration Configuration { get; } 


        public EmailSender(IConfiguration configuration)
        {
            Configuration = configuration;
        } 
        
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var sender = Configuration.GetSection("TESTING-SMTPClientDetails");
            var host = sender["host"]; // smtp.gmail.com
            int.TryParse(sender["Port"], out var port); // 587
            var userName = sender["UserName"]; // example@gmail.com
            var password = sender["Password"]; // P4ssw0rd
            
            var mm = new MailMessage()
            {
                From = new MailAddress("noreplay@earl-nav.com", "Earl Nav"),
                To = { new MailAddress(email) },
                Body = htmlMessage,
                Subject = subject,
                IsBodyHtml = true,
            };
            var client = new SmtpClient
            {
                Port = port,
                Host = host,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(userName,password),
            };
            
            return client.SendMailAsync(mm);
        }
    }
}