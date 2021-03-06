﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using LiteDB;
using Newtonsoft.Json.Linq;
using Server.Classes;
using Server.DbClass;


namespace Server.Controllers
{
    [Route("Handmade/[controller]")]
    public class Auth : Controller
    {
       
        public LiteDatabase db;
        public Auth()
        {
            db = new LiteDatabase(Tools._connectionString);
        }

        // GET: LoginUsers
        [HttpGet]
        public IEnumerable<User> Get()
        {
                var users = Tools.Database.GetCollection<User>("Users");
                var temp = users.FindAll();
          
                return temp;
        }

        // GET Login User with username password
        [HttpGet("{username}/{password}")]
        public JObject Get(string username , string password)
        {
            JObject jObject = new JObject();

            try
            {
                var users = Tools.Database.GetCollection<User>("Users");
                if (users.Exists(u => u.UserName .Equals(username)))
                    {
                    var user = users.FindOne(u => u.UserName.Equals(username));
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                jObject["result"] = $"User Login Faild : {ex.Message}";
                jObject["Code"] = 0;
                return jObject;

            }
        }


        /// <summary>
        /// Gets the user detail.
        /// </summary>
        /// <returns>The user detail.</returns>
        /// <param name="userName">User name.</param>
        /// <param name="passWord">Pass word.</param>
        [HttpGet]
        [Route("getDetail/{username}/{password}")]
        [ActionName("GetUserDetail")]
        public User GetUserDetail(string userName , string passWord)
        {

            try
            {
                var user = Tools.Database.GetCollection<User>(Tools.Users).FindOne(u => u.UserName.Equals(userName));

                if(!user.PassWord.Equals(passWord)){
                    throw new Exception("password is wrong");
                }

                return user;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Getting user detail error : {ex.Message}");
                return new User()
                {
                    UserName = ex.Message
                };

            }
        }
        /// <summary>
        /// Verify the specified username and code.
        /// </summary>
        /// <returns>The verify.</returns>
        /// <param name="username">Username.</param>
        /// <param name="code">Code.</param>
        //[Route("Handmade/[controller]/Verify")]
        [HttpPost("{username}/{code}")]
        public JObject Verify(string username,int code)
        {
            JObject result = new JObject();
            try
            {
                int temp;
             
                    var codes = Tools.Database.GetCollection<Verify>("Codes");
                    var users = Tools.Database.GetCollection<User>("Users");
                    var tempcode = codes.FindOne(c => c.UserName == username);
                    if (tempcode.IsVerified)
                    {
                        result["Result"] = "This code is used .";
                        result["Code"] = 0;
                        return result;

                    }
                    else
                    {
                        temp = tempcode.Code;
                        if (tempcode.Code == code)
                        {
                            tempcode.IsVerified = true;
                            codes.Update(tempcode);
                            var user = users.FindOne(u => u.UserName == username);
                            user.IsActive = true;
                            users.Update(user);
                            result["Result"] = "user verifivation is success.";
                            result["Code"] = 1;
                            return result;
                        }
                        else
                        {
                            result["Result"] = $"user verifivation is Faild : wrong code.[{code}]-[{temp}]";
                            result["Code"] = 0;
                            return result;

                        }
                    }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Verifing user faild :{e.Message}");
                result["Result"] = $"user verifivation is Faild : {e.Message}";
                result["Code"] = 0;
                return result;

            }
        }
        /// <summary>
        /// Post this instance.
        /// </summary>
        /// <returns>The post.</returns>
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
             //   var Images = HttpContext.Request.Form.Files[0]; this code for upload image form client

            
                    
                var users = Tools.Database.GetCollection<User>("users");
                var codes = Tools.Database.GetCollection<Verify>("Codes");
                   if(users.Exists(u=>u.UserName.Equals(userName))){
                    
                        Console.WriteLine($"username exist");
                       
                        jObject["result"] = "UserName exist";
                        jObject["Code"] = 0;
                        return jObject;
                    }
                    else{
                    Random random = new Random();
                    int randomNumber = random.Next(0, 5);

                        users.Insert(new User()
                        {
                            UserName = userName,
                            Mail = mail,
                            PhoneNumebr = phoneNumber,
                            PassWord=passWord,
                            IsActive = false
                            ,
                        UserImage = $"{randomNumber}.jpeg"
                        });

                    if (codes.Exists(c => c.UserName.Equals(userName)))
                        {
                        codes.Delete(c => c.UserName.Equals(userName));
                            
                        }
                    int temp =  Tools.SendVerifingCodeViaMail(userName, mail);
                        codes.Insert(new Verify()
                        {
                            UserName = userName,
                            Code = temp,
                            IsVerified = false
                        });
                 

                   //    Tools.SaveUserProfileImage(userName, Images); this code for upload image from client


                        jObject["Result"] = $"Registeration in success , code send to this mail {temp} - {mail}";
                        jObject["Code"] =1;
                        Console.WriteLine($"Registeration in success ,username: {userName} code send to this mail {mail}");
                        return jObject;
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
