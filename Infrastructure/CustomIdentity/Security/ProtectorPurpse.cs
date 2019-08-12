using ApplicationCore.Enumeration;

namespace Infrastructure.CustomIdentity.Security
{
    /// <summary>
    /// IDataProtector purposes
    /// </summary>
    public sealed class ProtectorPurpse
    {
        public static string MailConfirmation = "Mail";
        public static string PasswordRecovery = "Recovery";
    }
}
