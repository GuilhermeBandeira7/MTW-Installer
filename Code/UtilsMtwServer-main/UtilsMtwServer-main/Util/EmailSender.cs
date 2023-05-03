using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;


namespace UtilsCore
{
    public class EmailSender
    {
        #region SINGLETON

        private static EmailSender instance = null;

        private EmailSender()
        {

        }

        public static EmailSender Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EmailSender();
                }
                return instance;
            }
        }




        #endregion
        public void SendEmail(string smtpServer, string smtpPort, string senderEmail, string emailPassword, List<string> targetEmail, string title, string message)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(smtpServer);

                mail.From = new MailAddress(senderEmail);
                mail.To.Add(string.Join(",", targetEmail.ToArray()));
                mail.Subject = title;
                mail.Body = message;

                SmtpServer.Port = Convert.ToInt32(smtpPort);
                SmtpServer.Credentials = new NetworkCredential(senderEmail, emailPassword);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                Console.WriteLine("Email enviado.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
