using EmailSenderNet.Models;
using EmailSenderNet.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmailSenderNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailSendController : ControllerBase
    {
        private readonly IEmailSendService emailSendService;

        public EmailSendController(IEmailSendService emailSendService)
        {
            this.emailSendService = emailSendService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> SendMail(EmailSendDefault emailData)
        {
            var sendEmail = await emailSendService.SendMail(emailData);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> SendMailHtml(EmailSendHtml emailData)
        {
            var sendEmail = await emailSendService.SendMailHtml(emailData);
            return Ok();
        }
    }
}
