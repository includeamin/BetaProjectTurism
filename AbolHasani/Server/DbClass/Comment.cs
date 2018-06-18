using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DbClass
{
    public class Comment
    {
        public int Id { get; set; }
        public string LocationId { set; get; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string UserImage { set; get; }
    }
}
