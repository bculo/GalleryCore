using System;

namespace Infrastructure.Helpers.Mail
{
    public class PasswordRecoveryMail : MailFactory
    {
        protected override void GenerateContent(string url)
        {
            Subject = "Image Gallery password recovery";
            Content = $"Change your password by <a href='{ EncodeUrl(url) }'>clicking here</a>.";
        }
    }
}
