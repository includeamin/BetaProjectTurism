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
using Server.Classes;

namespace Server.Controllers
{
   // [Produces("application/json")]
    [Route("Handmade/Download")]
    public class DownloadController : Controller
    {
        /// <summary>
        /// Get the specified filename.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="filename">Filename.</param>
        [HttpGet("{filename}")]
        public RedirectResult Get(string filename)
        {
            Console.WriteLine($"Download Image from client {filename}");

            return Redirect(Tools.DownloadServerURl+$"?name={filename}");
        }

    }
}