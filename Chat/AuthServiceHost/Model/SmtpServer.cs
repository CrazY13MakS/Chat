using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AuthServiceProvider.Model
{
    public class SmtpServer
    {
        public SmtpServer(String host, String from, String password)
        {
            Host = host;
            From = from;
            Password = password;
            // SendMail("smtp.gmail.com", "crazy13maks@gmail.com", "", "klepach@itstep.org", "Tema", "Hello World!");
        }
        public String Host { get; set; }
        public String From { get; set; }
        public String Password { get; set; }

        public bool SendMail(string mailto, string caption, string message, string attachFile = null)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(From);
                mail.To.Add(new MailAddress(mailto));
                mail.Subject = caption;
                mail.Body = message;
                if (!string.IsNullOrEmpty(attachFile))
                    mail.Attachments.Add(new Attachment(attachFile));
                SmtpClient client = new SmtpClient
                {
                    Host = Host,
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(From.Split('@')[0], Password),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
                client.Send(mail);
                mail.Dispose();
            }
            catch (Exception e)
            {
                return false;// throw new Exception("Mail.Send: " + e.Message);
            }
            return true;
        }
    }
}
