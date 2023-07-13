using EmailSenderNet.Models;

namespace EmailSenderNet.Services
{
    public interface IEmailSendService
    {
        Task<bool> SendMail(EmailSendDefault emailData);
        Task<bool> SendMailHtml(EmailSendHtml emailHtmlData);
    }
}
