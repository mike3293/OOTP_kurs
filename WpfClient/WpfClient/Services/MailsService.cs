using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WpfClient.Auth;

namespace WpfClient.Services
{
    public class MailsService
    {
        public static async Task SendEmailAsync(string emailTo, string title, string displayName, string htmlBody = "")
        {
            string login = ConfigurationManager.AppSettings["emailLogin"];
            string password = ConfigurationManager.AppSettings["emailPass"];

            password = AesEncryption.DecryptString("b14ca5898a4e4133bbce2ea2315a1916", password);

            MailAddress from = new MailAddress(login, displayName);
            MailAddress to = new MailAddress(emailTo);
            MailMessage message = new MailMessage(from, to)
            {
                Subject = title,
                Body = htmlBody,
                IsBodyHtml = true
            };

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(login, password),
                EnableSsl = true,
            };

            await smtp.SendMailAsync(message);
        }
    }
}
