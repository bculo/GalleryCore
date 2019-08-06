using System.Text.Encodings.Web;

namespace Infrastructure.Helpers.Mail
{
    public abstract class MailFactory
    {
        public string Subject { get; protected set; }
        public string Content { get; protected set; }

        /// <summary>
        /// Generate content from mail
        /// </summary>
        /// <param name="url">url in mail that user can click (optional)</param>
        public void PrepareContent(string url = "")
        {
            GenerateContent(url);
        }

        /// <summary>
        /// Encode url using HTML encoder
        /// </summary>
        /// <param name="url">url to encode</param>
        /// <returns></returns>
        protected virtual string EncodeUrl(string url)
        {
            return HtmlEncoder.Default.Encode(url);
        }

        protected abstract void GenerateContent(string url);

        /// <summary>
        /// Create instance of MailFactory
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="url">url (optional)</param>
        /// <returns>instance of MailFactory</returns>
        public static MailFactory GetMailFactory<T>(string url = "") where T : MailFactory, new()
        {
            MailFactory mailFactory = new T();
            mailFactory.PrepareContent(url);
            return mailFactory;
        }
    }
}
