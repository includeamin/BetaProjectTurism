using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Server.Classes
{
    public static class Tools
    {
        public static int GenerateCode()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public static int SendVerifingCodeViaMail(string userName, string mail)
        {
            var apiKey = Environment.GetEnvironmentVariable("6F43714C41496A2B386F624931324F61494A323157736F6A73394B766B504871");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("Handmade@Handmade.com", "aminjamal");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress(userName, mail);
            var code = GenerateCode();
            var plainTextContent = $"{code}";
            var htmlContent = $"<strong>{code}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response =  client.SendEmailAsync(msg);

            return code;
        }
    }
}
