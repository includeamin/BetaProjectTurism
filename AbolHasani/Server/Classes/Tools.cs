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

        //URLS ========================================================
        public readonly static string DownloadServerURl = "http://178.63.169.234:9001/";
        public readonly static string MainServerURL = "http://[::]:9000/";


        //DataBase Connection ==========================================
        public readonly static ConnectionString _connectionString = new ConnectionString()
        {
            Mode = FileMode.Exclusive,
            Filename = "Hasani.db"
        };

        //Database collections name ====================================
        public static string DbName = "Hasani.db";
        public static string Locations = "Locations";
        public static string Users = "users";
        public static string Comments = "Comments";


        //Mail Configs =================================================
        public static string MailAddress = "handmade.development@gmail.com";
        public static string MailPassWord = "Mygmail44";
        public static string StmpServer = "smtp.gmail.com";
        public static int StmpServerPort = 587;






        public static LiteDatabase Database;
        static Tools()
        {
            Database = new LiteDatabase(_connectionString);
        }

        //Other tools ===================================================

        public static int GenerateCode()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        /// <summary>
        /// Sends the verifing code via mail.
        /// </summary>
        /// <returns>The verifing code via mail.</returns>
        /// <param name="userName">User name.</param>
        /// <param name="mail">Mail.</param>
        public static int SendVerifingCodeViaMail(string userName, string mail)
        {
           
            var code = GenerateCode();
            var client = new SmtpClient(StmpServer, StmpServerPort)
            {
                Credentials = new System.Net.NetworkCredential(MailAddress, MailPassWord),
                EnableSsl = true
            };

            client.Send(mail, mail, "Verifing user", code.ToString());
           

            return code;
        }

        /// <summary>
        /// Result the specified code and message.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="code">Code.</param>
        /// <param name="message">Message.</param>
        public static JObject Result(int code , string message){
            
            var temp = new JObject();
            temp["Code"] = code;
            temp["Result"] = message;
            return temp;
        }
        /// <summary>
        /// Saves the file to safe dir.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="file">File.</param>
        public static void SaveFileToSafeDir(string title ,Microsoft.AspNetCore.Http.IFormFile file){
            
            var stream = file.OpenReadStream();

           
            var temp = DateTime.Now.DayOfYear+DateTime.Now.Millisecond.ToString() + file.FileName;
        
            var filePath = Path.Combine("./UploadedFiles",temp);
            
            FileStream amin = new FileStream(filePath, System.IO.FileMode.OpenOrCreate);

            stream.CopyTo(amin);
          
                amin.Close();
                Console.WriteLine(title);
                var places = Database.GetCollection<Location>("Locations");
                var place = places.FindOne(t => t.Title == title);
                place.ImagesList.Add(temp);
                places.Update(place);


       
         
        }
        /// <summary>
        /// Saves the user profile image.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="file">File.</param>

        public static void SaveUserProfileImage(string username, Microsoft.AspNetCore.Http.IFormFile file){

            var stream = file.OpenReadStream();


            var temp = DateTime.Now.DayOfYear + DateTime.Now.Millisecond.ToString() + file.FileName;

            var filePath = Path.Combine("./UploadedFiles", temp);

            FileStream amin = new FileStream(filePath, System.IO.FileMode.OpenOrCreate);

            stream.CopyTo(amin);
            Console.WriteLine($"Upload  {username}'s profile pic {file.FileName}");
            amin.Close();
            var users = Database.GetCollection<User>("users");
            var user = users.FindOne(u => u.UserName.Equals(username));
            user.UserImage = temp;
            users.Update(user);
            Console.WriteLine("Done");

        }

        /// <summary>
        /// Hashs the user pass word.
        /// </summary>
        /// <returns>The user pass word.</returns>
        /// <param name="passWord">Pass word.</param>
   //     public string HashUserPassWord(string passWord)
   //     { 
     //       using(var sha256 = SHA256.Create())  
     //       {  
     
    //            varhashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passWord));  
      
     //           var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

      //          Console.WriteLine(hash);  

       //         return hash;
                      
      //      }  
            
      //  }
    }
}
