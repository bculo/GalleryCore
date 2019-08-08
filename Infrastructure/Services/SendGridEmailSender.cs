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
            CheckInputs(mail, callbackURL, token);
            MailFactory mailFactory = MailFactory.GetMailFactory<RegistrationActivationMail>(callbackURL);
            SendGridMessage msg = PrepareSendGridMessage(mailFactory, mail);
            return client.SendEmailAsync(msg);
        }

        public virtual Task SendPasswordRecoveryAsync(string mail, string callbackURL, string token)
        {
            CheckInputs(mail, callbackURL, token);
            MailFactory mailFactory = MailFactory.GetMailFactory<PasswordRecoveryMail>(callbackURL);
            SendGridMessage msg = PrepareSendGridMessage(mailFactory, mail);
            return client.SendEmailAsync(msg);
        }

        public SendGridMessage PrepareSendGridMessage(MailFactory factory, string email)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(mailConfiguration.ApplicationMail),
                Subject = factory.Subject,
                PlainTextContent = factory.Content,
                HtmlContent = factory.Content
            };

            msg.AddTo(new EmailAddress(email));
            msg.SetClickTracking(false, false);

            return msg;
        }

        /// <summary>
        /// Check input mail
        /// </summary>
        /// <param name="mail">mail</param>
        protected virtual void CheckInputs(string mail, string callbackUrl, string token)
        {
            if(string.IsNullOrWhiteSpace(mail))
            {
                throw new ArgumentNullException(nameof(mail));
            }

            if (string.IsNullOrWhiteSpace(callbackUrl))
            {
                throw new ArgumentNullException(nameof(callbackUrl));
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }
        }
    }
}
