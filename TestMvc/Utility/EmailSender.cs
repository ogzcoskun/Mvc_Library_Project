using Microsoft.AspNetCore.Identity.UI.Services;

namespace TestMvc.Utility
{
    public class EmailSender : IEmailSender
    {
        public EmailSender()
        {
            
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            // Email Verification istenirse smtp server ile yapılabilir.

            return Task.CompletedTask;
        }
    }
}
