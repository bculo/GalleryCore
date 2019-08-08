using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Email service interface
    /// </summary>
    public interface IEmailSender
    {
        Task SendRegistrationEmailAsync(string mail, string callbackURL, string token);
        Task SendPasswordRecoveryAsync(string mail, string callbackURL, string token);
    }
}
