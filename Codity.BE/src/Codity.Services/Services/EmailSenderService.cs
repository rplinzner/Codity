using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;
using Codity.Services.Interfaces;
using Codity.Services.Options;

namespace Codity.Services.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly EmailServiceOptions _emailOptions;

        public EmailSenderService(IOptions<EmailServiceOptions> emailOptions)
        {
            _emailOptions = emailOptions.Value;
        }

        public async Task<bool> SendEmail(string receiver, string title, string message)
        {
            try
            {
                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(_emailOptions.SenderName, _emailOptions.Sender));

                mimeMessage.To.Add(new MailboxAddress(receiver));

                mimeMessage.Subject = title;

                mimeMessage.Body = new TextPart("html")
                {
                    Text = message
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync(_emailOptions.MailServer, _emailOptions.MailPort, true);

                    await client.AuthenticateAsync(_emailOptions.Sender, _emailOptions.Password);

                    await client.SendAsync(mimeMessage);

                    await client.DisconnectAsync(true);
                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
