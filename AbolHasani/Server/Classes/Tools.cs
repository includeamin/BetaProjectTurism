using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore;

using LiteDB;
using Server.Controllers;
using Server.DbClass;
using FileMode = LiteDB.FileMode;

namespace Server.Classes
{
    public static class Tools
    {
        public readonly static ConnectionString _connectionString = new ConnectionString()
        {
            Mode = FileMode.Exclusive,
            Filename = "Hasani.db"
        };
        public static LiteDatabase Database;
        static Tools()
        {
            Database = new LiteDatabase(_connectionString);
        }
        public static int GenerateCode()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public static int SendVerifingCodeViaMail(string userName, string mail)
        {
           
            var code = GenerateCode();
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new System.Net.NetworkCredential("handmade.development@gmail.com", "Mygmail44"),
                EnableSsl = true
            };
            client.Send(mail, mail, "Verifing user", code.ToString());
           

            return code;
        }
        public static JObject Result(int code , string message){
            var temp = new JObject();
            temp["Code"] = code;
            temp["Result"] = message;
            return temp;
        }
        public static void SaveFileToSafeDir(string title ,Microsoft.AspNetCore.Http.IFormFile file){
            
            var stream = file.OpenReadStream();
           
            var temp = DateTime.Now.DayOfYear+DateTime.Now.Millisecond.ToString() + file.FileName;
        
            var filePath = Path.Combine("./UploadedFiles",temp);
            
            FileStream amin = new FileStream(filePath, System.IO.FileMode.CreateNew);

            stream.CopyTo(amin);
          //  using (var db = new LiteDatabase(_connectionString))
           // {
                Console.WriteLine(title);
                var places = Database.GetCollection<Location>("Locations");
                var place = places.FindOne(t => t.Title == title);
                place.ImagesList.Add(temp);
                places.Update(place);


          //  }

         
        }
    }
}
