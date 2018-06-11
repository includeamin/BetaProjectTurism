﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LiteDB;
using Newtonsoft.Json.Linq;
using Server.Classes;
using Server.DbClass;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("Handmade/[controller]")]
    public class Auth : Controller
    {
        readonly ConnectionString _connectionString = new ConnectionString(){
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
                   
                using (var db = new LiteDatabase(_connectionString))
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

        [Route("/Verify")]
        [HttpGet("{username}/{code}")]
        public void Verify(string username,int code)
        {
            JObject result = new JObject();
            try
            {
                using (var db = new LiteDatabase("Hasani.db"))
                {
                    var codes = db.GetCollection<Verify>("Codes");
                    var users = db.GetCollection<User>("Users");
                    var tempcode = codes.FindOne(c => c.UserName == username);
                    if (tempcode.IsVerified)
                    {
                        result["Result"] = "This code is used .";
                        result["Code"] = 0;

                    }
                    else
                    {
                        if (tempcode.Code == code)
                        {
                            tempcode.IsVerified = true;
                            codes.Update(tempcode);
                            var user = users.FindOne(u => u.UserName == username);
                            user.IsActive = true;
                            users.Update(user);
                            result["Result"] = "user verifivation is success.";
                            result["Code"] = 1;
                        }
                        else
                        {
                            result["Result"] = "user verifivation is Faild : wrong code.";
                            result["Code"] = 0;

                        }
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Verifing user faild :{e.Message}");
                
            }
        }

        // POST api/values
        [HttpPost]
        public JObject Post()
        {
            JObject jObject = new JObject();
            try{
                
                var from = HttpContext.Request.Form;
                var userName = from["UserName"];
                var mail = from["Mail"];
                var phoneNumber = from["PhoneNumber"];
                var passWord = from["PassWord"];


                using(var db= new LiteDatabase(_connectionString)){
                    
                    var users = db.GetCollection<User>("users");
                    var codes = db.GetCollection<Verify>("Codes");
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
                            PhoneNumebr = phoneNumber,
                            PassWord=passWord,
                            IsActive = false
                        });
                        if (codes.Exists(c => c.UserName == userName))
                        {
                            codes.Delete(c => c.UserName == userName);
                            
                        }
                        codes.Insert(new Verify()
                        {
                            UserName = userName,
                            Code = Tools.SendVerifingCodeViaMail(userName,mail),
                            IsVerified = false
                        });
                        jObject["Result"] = $"Registeration in success , code send to this mail {mail}";
                        jObject["Code"] =1;
                        Console.WriteLine($"Registeration in success ,username: {userName} code send to this mail {mail}");
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
            try
            {
                var form = HttpContext.Request.Form;
                var userName = form["UserName"];
                var editMode = form["editMode"];//mail , name , password
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
