using EmailSenderNet.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmailSenderNet.Services.Impl
{
    public class EmailSendService : IEmailSendService
    {
        private readonly EmailSettings _emailSettings;

        public EmailSendService(IOptions<EmailSettings> mailOption)
        {
            _emailSettings = mailOption.Value;
        }

        public async Task<bool> SendMail(EmailSendDefault emailData)
        {
            try
            {
                using (MimeMessage emailSend = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail);
                    emailSend.From.Add(emailFrom);

                    MailboxAddress emailTo = new MailboxAddress(emailData.EmailName, emailData.EmailAddress);
                    emailSend.To.Add(emailTo);

                    emailSend.Subject = emailData.EmailSubject;
                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.TextBody = emailData.EmailBody;

                    emailSend.Body = emailBodyBuilder.ToMessageBody();

                    using (var mailClient = new SmtpClient())
                    {
                        await mailClient.ConnectAsync(_emailSettings.Server, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        await mailClient.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
                        await mailClient.SendAsync(emailSend);
                        await mailClient.DisconnectAsync(true);
                    }
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> SendMailHtml(EmailSendHtml emailHtmlData)
        {
            try
            {
                using (MimeMessage emailSend = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail);
                    emailSend.From.Add(emailFrom);

                    MailboxAddress emailTo = new MailboxAddress(emailHtmlData.EmailName, emailHtmlData.EmailAddress);
                    emailSend.To.Add(emailTo);

                    emailSend.Subject = "New Message For You";
                    string filePath = Directory.GetCurrentDirectory() + "\\Public\\Template\\information-schedule.html";
                    string emailTemplateText = File.ReadAllText(filePath);

                    string replacingHtml = emailTemplateText.Replace("{", "{{").Replace("}", "}}");
                    replacingHtml = string.Format(replacingHtml, emailHtmlData.EmailName, DateTime.Today.ToShortDateString());

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = replacingHtml;

                    emailSend.Body = emailBodyBuilder.ToMessageBody();

                    using (var mailClient = new SmtpClient())
                    {
                        await mailClient.ConnectAsync(_emailSettings.Server, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        await mailClient.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
                        await mailClient.SendAsync(emailSend);
                        await mailClient.DisconnectAsync(true);
                    }
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
