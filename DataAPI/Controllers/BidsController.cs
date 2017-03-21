using Frostmourne_basics;
using Frostmourne_basics.Dbs;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using xAPI.Commands;
using xAPI.Sync;

namespace DataAPI.Controllers
{
    public partial class BidsController : ApiController
    {
        // GET api/bids/details
        /*
        [HttpGet]
        public IEnumerable<string> Details()
        {
            return new string[] { "value1" };
        }

        [HttpGet]
        public string Details(string arg1)
        {
            return "arg1 -> " + arg1;
        }*/
        
        [HttpGet]
        public Error Update_Symbols(string arg1)
        {
            Error err;
            Mysql MyDB = new Mysql();
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration();
            configuration.LoadAPIConfigurationSettings();
            
            err = Tool.Init(ref Xtb_api_connector, ref configuration, ref MyDB);
            if (err.IsAnError)
            {
                return err;
            }

            int days = Convert.ToInt32(arg1);

            if (days <= 0)
                days = 1;

            if (days > 30)
                days = 30;

            err = Xtb.Retrieve_and_update_data_for_symbols(MyDB, Xtb_api_connector, days, 0);
            if (err.IsAnError)
            {
                return err;
            }

            return new Error(false, "All non inactive symbols updated");
        }

        [HttpGet]
        public Error Update_Symbol(string arg1, string arg2)
        {
            Error err;
            Mysql MyDB = new Mysql();
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration();
            configuration.LoadAPIConfigurationSettings();

            err = Tool.Init(ref Xtb_api_connector, ref configuration, ref MyDB);
            if (err.IsAnError)
            {
                return err;
            }

            int days = Convert.ToInt32(arg2);
            
            if (days <= 0)
                days = 1;

            if (days > 30)
                days = 30;

            err = Xtb.Retrieve_and_update_data_for_symbol(MyDB, Xtb_api_connector, new Symbol(0, arg1,""), days, 0);
            if (err.IsAnError)
            {
                return err;
            }

            return new Error(false, "Data Updated");
        }
    }
}
