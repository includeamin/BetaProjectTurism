using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LiteDB;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("Handmade/[controller]")]
    public class Auth : Controller
    {
        ConnectionString connectionString = new ConnectionString(){
            Mode= FileMode.Exclusive,
            Filename = "Hasani.db"
        };
       
        // GET: LoginUsers
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET Login User with username password
        [HttpGet("{username}/{password}")]
        public JObject Get(string username , string password)
        {
            JObject jObject = new JObject();

            try
            {
               
               
                using (var db = new LiteDatabase(connectionString))
                {
                    
                    var users = db.GetCollection<User>("Users");
                    if (users.Exists(u => u.UserName == username))
                    {
                        var user = users.FindOne(u => u.UserName == username);
                        if (user.PassWord == password)
                        {
                            jObject["result"] = "User Login Success";
                            jObject["Code"] = 1;
                            return jObject;
                        }
                        else{
                            jObject["result"] = "User Login Faild : Password is Wrong";
                            jObject["Code"] = 0;
                            return jObject;
                        }


                    }else{
                        jObject["result"] = "User Login Faild : use with this username not found";
                        jObject["Code"] = 0;
                        return jObject;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                jObject["result"] = $"User Login Faild : {ex.Message}";
                jObject["Code"] = 0;
                return jObject;

            }
        }

        // POST api/values
        [HttpPost]
        public JObject Post([FromBody]string value)
        {
            JObject jObject = new JObject();
            try{
                
                var from = HttpContext.Request.Form;
                var userName = from["UserName"];
                var mail = from["Mail"];
                var PhoneNumber = from["PhoneNumber"];
                var passWord = from["PassWord"];


                using(var db= new LiteDatabase(connectionString)){
                    
                    var users = db.GetCollection<User>("users");
                    if(users.Exists(u=>u.UserName == userName)){
                        Console.WriteLine($"username exist");
                       
                        jObject["result"] = "UserName exist";
                        jObject["Code"] = 0;
                        return jObject;
                    }
                    else{
                        users.Insert(new User()
                        {
                            UserName = userName,
                            Mail = mail,
                            PhoneNumebr = PhoneNumber,
                            PassWord=passWord
                        });
                        jObject["Result"] = "Registeration in success";
                        jObject["Code"] =1;
                        return jObject;
                    }

                }

            }catch(Exception e){
                Console.WriteLine($"error while registeration {e.Message}");
                jObject["Result"] = $"Registeration Faild : {e.Message}";
                jObject["Code"] = 0;
                return jObject;
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
