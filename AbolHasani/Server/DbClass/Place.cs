using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DbClass
{
    public class Place
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { set; get; }
        public List<Comment> Comments { get; set; }
        public int Star { set; get; }
        

    }
}
