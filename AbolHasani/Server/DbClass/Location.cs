using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DbClass
{
    public class Location
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Lat { set; get; }
        public double Long { set; get; }
        public double Star { set; get; }
        public string State { get; set; }
        public string City { get; set; }
        public List<string> ImagesList { set; get; }

    }
}
