using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
   // [Produces("application/json")]
    [Route("Handmade/Download")]
    public class DownloadController : Controller
    {
        [HttpGet("{filename}")]
        public HttpResponseMessage Get(string filename)
        {
            
            // processing the stream.
          //  var file = System.IO.File.OpenRead($"./UploadedFiles/{filename}");
          //  byte[] bytes = new byte[file.Length];
          //  var stream = new MemoryStream(bytes, 0, bytes.Length);

            //var result = new HttpResponseMessage(HttpStatusCode.OK)
            //{
            //    Content = new ByteArrayContent(stream.ToArray())
            //};
            //result.Content.Headers.ContentDisposition =
            //    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            //    {
            //        FileName = filename
            //    };
            //result.Content.Headers.ContentType =
            //    new MediaTypeHeaderValue("application/octet-stream");

            //return result;
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new System.IO.FileStream($"./UploadedFiles/{filename}", System.IO.FileMode.Open);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return response;
        }

    }
}