using SendGrid;
using SendGrid.Helpers.Mail;

namespace ASP.NET_DnD_App.Models
{
    public interface IEmailProvider
    {
        Task<Response> SendEmailAsync(string username, string toEmail, string fromEmail, string subject, string body, string htmlContent); 
    }

    public class SendGridEmailProvider : IEmailProvider
    {

        private readonly IConfiguration _config;
        public SendGridEmailProvider(IConfiguration config)
        {
            _config = config;
        }

        public async Task<Response> SendEmailAsync(string username, string toEmail, string fromEmail, string subject, string body, string htmlContent)
        {
            var apiKey = _config.GetSection("SENDGRID_KEY").Value;
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, "Dungeons & Dragons Character Manager Team"),
                Subject = subject,
                HtmlContent = username + ", please confirm your email " + htmlContent
            };
            msg.AddTo(new EmailAddress(toEmail, username));
            var response = await client.SendEmailAsync(msg);
            return response;
        }
    }
}
