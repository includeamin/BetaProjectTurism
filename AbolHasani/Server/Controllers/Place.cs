using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LiteDB;
using Server.DbClass;
using Newtonsoft.Json.Linq;
using Server.Classes;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Web;
using System.Net;
using Microsoft.AspNetCore.Hosting;
namespace Server.Controllers
{
    [Route("Handmade/[controller]")]
    public class Place : Controller
    {
       
      
       
        // GET: api/values
        [HttpGet]
        public IEnumerable<Location> Get()
        {
           // using (var db = new LiteDatabase(Tools._connectionString))
           // {
                return Tools.Database.GetCollection<Location>("Locations").FindAll();
           // }

        }

        // GET api/values/5[
        [ActionName("GetLocationByusername")]
        [HttpGet("{username}/{title}")]
        public Location Get(string username,string title)
        {
            try
            {
                var locations = Tools.Database.GetCollection<Location>("Locations");

                var TempLcoation = locations.FindOne(t => t.Title.Equals(title));

                if(TempLcoation.UserLikedList.Exists(u=>u.Equals(username))){
                    TempLcoation.IsLiked = true;
                }



                return TempLcoation;
            }
            catch (Exception ex)
            {
                return new Location()
                {
                    Title = ex.Message
                };
            }
          
        }

        // POST api/values
        [HttpPost]
        public JObject Post()
        {
            try
            {
                var form = HttpContext.Request.Form;
                var lat = form["Lat"];
                var Long = form["Long"];
                var title = form["Title"];
                var description = form["Description"];
                var files = form.Files;
                var state = form["State"];
                var city = form["City"];
               

                     
                var locations = Tools.Database.GetCollection<Location>("Locations");
                if (locations.Exists(L => L.Title.Equals(title)))
                    {

                        return Tools.Result(0, "Location with this title is exist");
                    }
                    else
                    {
                        locations.Insert(new Location()
                        {
                            Lat = Convert.ToDouble(lat),
                            Long = Convert.ToDouble(Long),
                            Title = title,
                            Description = description,
                            ImagesList = new List<string>(),
                            UserLikedList = new List<string>()
                            
                            
                    });

                        foreach(var i in files){
                            Tools.SaveFileToSafeDir(title, i);
                        }

                        return Tools.Result(1, "Location  Added");
                }
          
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Upload place faild : {ex.Message}");
                return Tools.Result(0, $"Upload place faild : {ex.Message}");

            }
}

        // PUT api/values/5
        [HttpPut("{city}")]
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
