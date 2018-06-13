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
using System.Web;

namespace Server.Controllers
{
   // [Produces("application/json")]
    [Route("Handmade/Download")]
    public class DownloadController : Controller
    {
        [HttpGet("{filename}")]
        public RedirectResult Get(string filename)
        {
            Console.WriteLine(filename);

            return Redirect($"http://192.168.1.50:9001/?name={filename}");
        }

    }
}