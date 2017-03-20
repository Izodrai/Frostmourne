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
        public Error Update_Not_Inactiv_Symbols()
        {
            Error err;
            Mysql MyDB = new Mysql();
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration();
            configuration.LoadConfigurationSettings();

            err = Tool.Init(ref Xtb_api_connector, ref configuration, ref MyDB);
            if (err.IsAnError)
            {
                return err;
            }

            err = Xtb.Retrieve_and_update_data_for_symbols(MyDB, Xtb_api_connector, 0, 1);
            if (err.IsAnError)
            {
                return err;
            }

            return new Error(false, "All non inactive symbols updated");
        }

        [HttpGet]
        public Error Update_Not_Inactiv_Symbols(string arg1, string arg2)
        {
            Error err;
            Mysql MyDB = new Mysql();
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration();
            configuration.LoadConfigurationSettings();
            
            err = Tool.Init(ref Xtb_api_connector, ref configuration, ref MyDB);
            if (err.IsAnError)
            {
                return err;
            }

            err = Xtb.Retrieve_and_update_data_for_symbols(MyDB, Xtb_api_connector, Convert.ToInt32(arg1), Convert.ToInt32(arg2));
            if (err.IsAnError)
            {
                return err;
            }

            return new Error(false, "All non inactive symbols updated");
        }

        [HttpGet]
        public Error Update_Symbol(string arg1)
        {
            Error err;
            Mysql MyDB = new Mysql();
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration();
            configuration.LoadConfigurationSettings();

            err = Tool.Init(ref Xtb_api_connector, ref configuration, ref MyDB);
            if (err.IsAnError)
            {
                return err;
            }

            err = Xtb.Retrieve_and_update_data_for_symbol(MyDB, Xtb_api_connector, new Symbol(0, arg1, ""), 0, 1);
            if (err.IsAnError)
            {
                return err;
            }

            return new Error(false, "Data Updated");
        }

        [HttpGet]
        public Error Update_Symbol(string arg1, string arg2, string arg3)
        {
            Error err;
            Mysql MyDB = new Mysql();
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration();
            configuration.LoadConfigurationSettings();

            err = Tool.Init(ref Xtb_api_connector, ref configuration, ref MyDB);
            if (err.IsAnError)
            {
                return err;
            }

            err = Xtb.Retrieve_and_update_data_for_symbol(MyDB, Xtb_api_connector, new Symbol(0, arg1,""), Convert.ToInt32(arg2), Convert.ToInt32(arg3));
            if (err.IsAnError)
            {
                return err;
            }

            return new Error(false, "Data Updated");
        }



    }
}
