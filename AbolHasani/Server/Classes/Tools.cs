using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Classes
{
    public static class Tools
    {
        public static int GenerateCode()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public static int SendVerifingCodeViaMail(string userName, string mail)
        {
            return 1;
        }
    }
}
