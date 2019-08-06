namespace Infrastructure.Helpers.Mail
{
    public class RegistrationActivationMail : MailFactory
    {
        /// <summary>
        /// Generate content for registration activation
        /// </summary>
        /// <param name="url">url registration link</param>
        protected override void GenerateContent(string url)
        {
            Subject = "Image Gallery Registration";
            Content = $"Please confirm your account by <a href='{ EncodeUrl(url) }'>clicking here</a>.";
        }
    }
}
