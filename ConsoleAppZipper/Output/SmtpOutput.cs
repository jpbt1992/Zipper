using System;
using System.Net;
using System.Net.Mail;

namespace ConsoleAppZipper.Output
{
    public class SmtpOutput : IOutput
    {
        private readonly string smtpServer;
        private readonly string senderEmail;
        private readonly string smtpPassword;
        private readonly string recipientEmail;

        public SmtpOutput
            (string smtpServer,
            string senderEmail,
            string smtpPassword,
            string recipientEmail)
        {
            this.smtpServer = smtpServer;
            this.senderEmail = senderEmail;
            this.smtpPassword = smtpPassword;
            this.recipientEmail = recipientEmail;
        }

        public void Save(string zipFilePath)
        {
            using SmtpClient client = new SmtpClient(smtpServer, 587);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(senderEmail, smtpPassword);
            client.EnableSsl = true;

            using (MailMessage message = new MailMessage(senderEmail, recipientEmail))
            {
                message.Subject = "Zip Attachment";
                message.Body = "Please find the attached ZIP file.";
                message.Attachments.Add(new Attachment(zipFilePath));
                message.IsBodyHtml = false;

                client.Send(message);
            }

            Console.WriteLine($"ZIP file sent as an email attachment via SMTP to {recipientEmail}");
        }
    }
}