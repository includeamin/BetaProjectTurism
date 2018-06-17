using System;
namespace Server.DbClass
{
    public class Likes
    {
       public int Id
        {
            get;
            set;
        }

        public string UserName { set; get; }
        public string LikedItemId { get; set; }


    
    }
}
