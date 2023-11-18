using Demo.DAL.Entities;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helper
{
    public class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);

            client.EnableSsl = true;

            client.Credentials = new NetworkCredential("andrewmorks@gmail.com", "wnaiebrcmktnmhmt");

            client.Send("andrewmorks@gmail.com", email.To, email.Title, email.Body);
        }
    }
}
