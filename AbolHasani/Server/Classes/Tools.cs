using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;




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
            //var apiKey = Environment.GetEnvironmentVariable("6F43714C41496A2B386F624931324F61494A323157736F6A73394B766B504871");
            //var client = new SendGridClient(apiKey);
            //var from = new EmailAddress("handmade.development@gmail.com", "handmade.development");
            //var subject = "Sending with SendGrid is Fun";
            //var to = new EmailAddress(userName, mail);
            //var code = GenerateCode();
            //var plainTextContent = $"{code}";
            //var htmlContent = $"<strong>{code}</strong>";
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            //var response =  client.SendEmailAsync(msg);
        /*    MailMessage Mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            var code = GenerateCode();
            Mail.From = new MailAddress("handmade.development@gmail.com");
            Mail.To.Add(mail);
            Mail.Subject = "VeryFing code";
            Mail.Body = code.ToString();

            SmtpServer.Port = 465;
            SmtpServer.Credentials = new System.Net.NetworkCredential("handmade.development@gmail.com", "Mygmail44");
            SmtpServer.EnableSsl = tru;

            SmtpServer.Send(Mail);*/
            var code = GenerateCode();
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new System.Net.NetworkCredential("handmade.development@gmail.com", "Mygmail44"),
                EnableSsl = true
            };
            client.Send(mail, mail, "Verifing user", code.ToString());
           

            return code;
        }
    }
}
