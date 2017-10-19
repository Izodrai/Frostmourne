using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAPI
{
    public class Token_validation
    {
        public static Boolean TokenValid(ref Frostmourne_basics.Configuration Config, string token)
        {
            if (token == Config.Api_token)
                return true;

            return false;
        }
    }
}

    