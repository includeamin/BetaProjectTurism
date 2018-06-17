using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Classes;
using Server.DbClass;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("Handmade/Comments")]
    public class CommentController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

    
        [HttpGet]
        [Route("usercomment/{username}")]
        [ActionName("GetUserComment")]
        public IEnumerable<Comment> Get(string  username)
        {
            try
            {
                var comments = Tools.Database.GetCollection<Comment>("Comments");
                var userComment = comments.Find(uc => uc.UserName.Equals(username));
                return userComment;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get users comment faild :{ex.Message}");
                return new Comment[]{
                    new Comment(){LocationTitle=$"{ex.Message}"}
                };
            }
        }

        [HttpGet]
        [Route("locationcommnet/{locationTitle}")]
        [ActionName("LocationComment")]
        public IEnumerable<Comment> GetLocationComment(string locationTitle)
        {
           try
            {
                var comments = Tools.Database.GetCollection<Comment>("Comments");
                var LocationComment = comments.Find(lc => lc.LocationTitle.Equals(locationTitle));
               
                return LocationComment;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get users comment faild :{ex.Message}");
                return new Comment[]{
                    new Comment(){LocationTitle=$"{ex.Message}"}
                };
            }
        }

        // POST api/values
        [HttpPost]
        public JObject Post()
        {
            var form = HttpContext.Request.Form;

            try
            {

                var description = form["Description"];
                var userName = form["UserName"];
                var locationTitle = form["Title"];



                if(userName==string.Empty){
                    throw new Exception("user name empty");
                }
                if (locationTitle == string.Empty)
                {
                    throw new Exception("locationTitle empty");
                }
             


                var comments = Tools.Database.GetCollection<Comment>("Comments");
                var users = Tools.Database.GetCollection<User>("Users");
                if(!users.Exists(u=>u.UserName.Equals(userName))){
                    throw new Exception("Username not found for this username");
                }

                var tempComment = new Comment()
                {

                    UserName = userName,
                    Description = description,
                    LocationTitle = locationTitle
                };

              comments.Insert(tempComment);
              return Tools.Result(1, "Comment Added");
        


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Tools.Result(0, $"Adding comment faild" + ex.Message);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public JObject Delete(string username,string title)
        {
           try
            {
                var comments = Tools.Database.GetCollection<Comment>("Comments");
                comments.Delete(c => c.UserName.Equals(username)&& c.LocationTitle.Equals(title));
                return Tools.Result(1, $"Comment deleted");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete commment faild :{ex.Message}");
                return Tools.Result(0, $"deleation faild:{ex.Message}");
            }
        }
    }
}
