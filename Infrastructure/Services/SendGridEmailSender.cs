using ApplicationCore.Interfaces;
using Infrastructure.Helpers.Mail;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// Send grid email service for sending emails
    /// </summary>
    public class SendGridEmailSender : IEmailSender
    {
        protected readonly MailOptions mailConfiguration;
        protected readonly SendGridClient client;

        public SendGridEmailSender(IOptions<MailOptions> optionsAccessor)
        {
            mailConfiguration = optionsAccessor.Value;
            client = new SendGridClient(mailConfiguration.Key);
        }

        public virtual Task SendRegistrationEmailAsync(string mail, string callbackURL, string token)
        {
            CheckInputMail(mail);
            CheckCallbackURL(callbackURL);

            MailFactory mailFactory = MailFactory.GetMailFactory<RegistrationActivationMail>(callbackURL);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(mailConfiguration.ApplicationMail),
                Subject = mailFactory.Subject,
                PlainTextContent = mailFactory.Content,
                HtmlContent = mailFactory.Content
            };

            msg.AddTo(new EmailAddress(mail));
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg); //DONT WAIT
        }

        /// <summary>
        /// Check input mail
        /// </summary>
        /// <param name="mail">mail</param>
        protected virtual void CheckInputMail(string mail)
        {
            if(string.IsNullOrWhiteSpace(mail))
            {
                throw new ArgumentNullException(nameof(mail));
            }
        }

        /// <summary>
        /// Check callback url
        /// </summary>
        /// <param name="mail">callback url</param>
        protected virtual void CheckCallbackURL(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }
        }
    }
}
