using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using LmsBeApp_Group06.Options;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace LmsBeApp_Group06.Services
{
    public class MailService
    {
        private readonly MailOption mailSettings;
        private readonly ILogger<MailService> logger;

        public MailService(IOptions<MailOption> mailOption, ILogger<MailService> logger)
        {
            this.logger = logger;
            mailSettings = mailOption.Value;
        }

        public async Task SendMail(MailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
            email.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
            email.To.Add(MailboxAddress.Parse(mailContent.To));
            email.Subject = mailContent.Subject;


            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;
            email.Body = builder.ToMessageBody();

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            smtp.Disconnect(true);

            logger.LogInformation("send mail to " + mailContent.To);

        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await SendMail(new MailContent()
            {
                To = email,
                Subject = subject,
                Body = htmlMessage
            });
        }
    }
}