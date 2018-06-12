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
        readonly ConnectionString _connectionString = new ConnectionString()
        {
            Mode = LiteDB.FileMode.Shared,
            Filename = "Hasani.db"
        };
      
       
        // GET: api/values
        [HttpGet]
        public IEnumerable<Location> Get()
        {

            using (var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<Location>("Locations").FindAll();
            }
            //var db = new LiteDatabase(_connectionString);
            //var data =  db.FileStorage.Find("KingAmin");
            //var stream1 = data.First().OpenRead();
            //byte[] bytes = new byte[stream1.Length];
            //stream1.Read(bytes, 0, (int)stream1.Length);


            //var stream = new MemoryStream(bytes);
            //// processing the stream.


            //var result = new HttpResponseMessage(HttpStatusCode.OK)
            //{
            //    Content = new ByteArrayContent(stream.ToArray())
            //};
            //result.Content.Headers.ContentDisposition =
            //    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            //    {
            //        FileName = data.First().Filename
            //    };
            //result.Content.Headers.ContentType =
            //    new MediaTypeHeaderValue("application/octet-stream");

            //return result;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
                var file = files[0];
                //  Tools.SaveFileToSafeDir(Title, file);
             //   var stream = file.OpenReadStream();
               

               // Tools.SaveFileToSafeDir(title,title,file);

                Console.WriteLine($"{file.FileName}-{file.ContentType}");



                //var carpath = Directory.GetCurrentDirectory();
              
               
                //Console.WriteLine(carpath);
                //var uploads = Path.Combine(carpath ,"UploadedFile");


                //var filePath = Path.Combine("./UploadedFiles", file.FileName);

                //Console.WriteLine(filePath);

                //FileStream amin = new FileStream(filePath, System.IO.FileMode.CreateNew);

                //stream.CopyTo(amin);


              




                using (var db = new LiteDatabase(_connectionString))
                {
                  //  db.FileStorage.Upload(Title,"amin.png", stream);
                    var locations = db.GetCollection<Location>("Locations");
                    if (locations.Exists(L => L.Title == title))
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
                            Comments = new List<Comment>()
                            
                    });
                        Tools.SaveFileToSafeDir(title, title, file);

                        return Tools.Result(1, "Location  Added");
                }
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
