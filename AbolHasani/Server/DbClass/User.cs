using System;
using System.Collections.Generic;
namespace Server
{
    public class User
    {
        
        //Fields
        public int Id{get;set;}
        public string UserName{ get; set; }
        public string Mail {get; set;  }
        public string PhoneNumebr { get; set; }
        public string PassWord { set; get; }
        public bool IsActive { set; get; }



    }
    
}
