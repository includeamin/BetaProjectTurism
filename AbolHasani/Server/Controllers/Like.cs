using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Classes;
using Server.DbClass;
using Newtonsoft.Json.Linq;



namespace Server.Controllers
{
    [Route("Handmade/[controller]")]
    public class Like : Controller
    {
       
        // POST api/values
        [HttpPost]
        public JObject Post()
        {
            var form = HttpContext.Request.Form;
            try
            {
                var username = form["UserName"];// should check thgis user in sexist or not
                var ItemId = form["ItemId"]; // locations title
                var Locations = Tools.Database.GetCollection<Location>("Locations");
                var TempLocation = Locations.FindOne(l => l.Title.Equals((string)ItemId));

             



                if (TempLocation.UserLikedList.Exists(u=> u==username)) {
                    
                   
                    TempLocation.LikesCount -= 1;
                    TempLocation.UserLikedList.Remove(username);
                    Locations.Update(TempLocation);

                    return Tools.Result(0, $"Unliked");

                    
                }else{
                    
                    TempLocation.LikesCount += 1;
                    TempLocation.UserLikedList.Add(username);
                    Locations.Update(TempLocation);

                    return Tools.Result(1, "Liked");
                }

              
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Like item faild :{ex.Message}");
                return Tools.Result(0, $"Like item faild :{ex}");
            }
        }

       
    }
}
