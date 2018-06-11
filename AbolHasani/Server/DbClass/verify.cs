using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DbClass
{
    public class Verify
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Code { get; set; }
        public bool IsVerified { get; set; }
    }
}
