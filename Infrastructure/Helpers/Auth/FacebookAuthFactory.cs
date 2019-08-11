namespace Infrastructure.Helpers.Auth
{
    public class FacebookAuthFactory : ExternalAuthFactory
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
