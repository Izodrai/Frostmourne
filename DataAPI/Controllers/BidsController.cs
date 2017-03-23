using DataAPI.Models;
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
        [HttpGet]
        public Response Update_Symbols(string arg1)
        {
            Error err;
            Mysql MyDB = new Mysql();
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration();
            configuration.LoadAPIConfigurationSettings();

            err = Tool.InitAll(ref Xtb_api_connector, ref configuration, ref MyDB);
            if (err.IsAnError)
                return new Response(err, null);

            DateTime tNow = DateTime.Now;
            DateTime tFrom = DateTime.Parse(arg1);

            List<Bid> bids = new List<Bid>();

            err = Xtb.Retrieve_and_update_data_for_symbols(MyDB, Xtb_api_connector, tNow, tFrom, ref bids);
            if (err.IsAnError)
                return new Response(err, null);

            return new Response(new Error(false, "All non inactive symbols updated, tFrom -> " + tFrom + " -> to -> tNow -> " + tNow), null);
        }

        [HttpGet]
        public Response Update_Symbol(string arg1, string arg2)
        {
            Error err;
            Mysql MyDB = new Mysql();
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration();
            configuration.LoadAPIConfigurationSettings();

            err = Tool.InitAll(ref Xtb_api_connector, ref configuration, ref MyDB);
            if (err.IsAnError)
                return new Response(err, null);
            

            DateTime tNow = DateTime.Now;
            DateTime tFrom = DateTime.Parse(arg2);

            List<Bid> bids = new List<Bid>();

            err = Xtb.Retrieve_and_update_data_for_symbol(MyDB, Xtb_api_connector, new Symbol(0, arg1, ""), tNow, tFrom, ref bids);
            if (err.IsAnError)
                return new Response(err, null);

            return new Response(new Error(false, "Data for symbol " + arg1 + " updated"), null);
        }

        [HttpGet]
        public Response Get_Symbol(string arg1, string arg2)
        {
            Error err;
            Mysql MyDB = new Mysql();
            Configuration configuration = new Configuration();
            configuration.LoadAPIConfigurationSettings();

            err = Tool.InitMyDb(ref configuration, ref MyDB);
            if (err.IsAnError)
                return new Response(err, null);

            DateTime tNow = DateTime.Now;
            DateTime tFrom = DateTime.Parse(arg2);
            List<Bid> bids = new List<Bid>();

            err = Xtb.Load_bids_symbol(MyDB, new Symbol(0, arg1, ""), tNow, tFrom, ref bids);
            if (err.IsAnError)
                return new Response(err, null);

            return new Response(new Error(false, "Data ok"), bids);
        }

        [HttpGet]
        public Response Get_Symbols(string arg1)
        {
            Error err;
            Mysql MyDB = new Mysql();
            Configuration configuration = new Configuration();
            configuration.LoadAPIConfigurationSettings();

            err = Tool.InitMyDb(ref configuration, ref MyDB);
            if (err.IsAnError)
                return new Response(err, null);

            DateTime tNow = DateTime.Now;
            DateTime tFrom = DateTime.Parse(arg1);
            List<Bid> bids = new List<Bid>();

            err = Xtb.Load_bids_symbols(MyDB, tNow, tFrom, ref bids);
            if (err.IsAnError)
                return new Response(err, null);

            return new Response(new Error(false, "Data ok"), bids);
        }
    }
}
