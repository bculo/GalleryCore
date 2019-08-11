namespace Infrastructure.Helpers.Auth
{
    public class GoogleAuthFactory : ExternalAuthFactory
    {
        protected override string IdentifierCriteria
        {
            get => "nameidentifier";
        }

        protected override string EmailCriteria
        {
            get => "emailaddress";
        }
    }
}
