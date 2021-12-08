using System;
using System.Net.Mail;

namespace HelperDotNet
{
    /// <summary>
    /// Helps send smtp mail
    /// </summary>
    public sealed class MailHelper
    {
        readonly string defaultSender = "";
        readonly SmtpClient client;

        Logger logger = null;
        public bool logToDB = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="smtpDomain">Smtp address</param>
        /// <param name="smtpPort">Smtp port</param>
        /// <param name="smtpId">Smtp id</param>
        /// <param name="smtpPassword">Smtp password</param>
        public MailHelper(string smtpDomain, int smtpPort, string smtpId, string smtpPassword)
        {
            client = new SmtpClient(smtpDomain, smtpPort);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(smtpId, smtpPassword);
            defaultSender = smtpId;
        }

        public void SetLogger(Logger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Send mail
        /// </summary>
        /// <param name="subject">Mali title</param>
        /// <param name="body">Mail body</param>
        /// <param name="to">Mail to list</param>
        /// <param name="from">Mail from</param>
        public void Send(string subject, string body, string[] to, string from = "")
        {
            if (from == "") from = defaultSender;

            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(from);

            string toS = "";

            foreach (string str in to)
            {
                message.To.Add(new MailAddress(str));
                toS += str + ";";
            }

            message.Subject = subject;
            message.Body = body;                        

            try
            {
                client.Send(message);
            }
            catch (Exception e)
            {
                if (logToDB)
                {
                    logger.WriteDB("Mail Sent to " + toS + " failed", "Subject : " + subject + " /Body : " + body + " /ErrorMsg : " + e.ToString());
                }
                else
                {
                    logger.WriteText("Mail Sent to " + toS + " failed /Subject : " + subject + " /Body : " + body + " /ErrorMsg : " + e.ToString());
                }
            }
            finally
            {
                if (logToDB)
                {
                    logger.WriteDB("Mail Sent to " + toS, "Subject : " + subject + " /Body : " + body);
                }
                else
                {
                    logger.WriteText("Mail Sent to " + toS + " /Subject : " + subject + " /Body : " + body);
                }
            }
        }

        /// <summary>
        /// Send mail
        /// </summary>
        /// <param name="subject">Mali title</param>
        /// <param name="body">Mail body</param>
        /// <param name="to">Mail to</param>
        /// <param name="from">Mail from</param>
        public void Send(string subject, string body, string to, string from = "")
        {
            Send(subject, body, new string[] { to }, from);
        }
    }
}
