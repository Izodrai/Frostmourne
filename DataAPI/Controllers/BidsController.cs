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
                return new Response(err, null, null);

            DateTime tNow = DateTime.UtcNow;
            DateTime tFrom = DateTime.Parse(arg1);

            List<Bid> bids = new List<Bid>();

            err = Xtb.Retrieve_and_update_data_for_symbols(MyDB, Xtb_api_connector, tNow, tFrom, ref bids, configuration);
            if (err.IsAnError)
                return new Response(err, null, null);

            return new Response(new Error(false, "All non inactive symbols updated, tFrom -> " + tFrom.ToString("yyyy-MM-dd HH:mm:ss") + " -> to -> tNow -> " + tNow.ToString("yyyy-MM-dd HH:mm:ss")), null, null);
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
                return new Response(err, null, null);
            
            DateTime tNow = DateTime.UtcNow;
            DateTime tFrom = DateTime.Parse(arg2);

            List<Bid> bids = new List<Bid>();

            err = Xtb.Retrieve_and_update_data_for_symbol(MyDB, Xtb_api_connector, new Symbol(0, arg1, ""), tNow, tFrom, ref bids, configuration);
            if (err.IsAnError)
                return new Response(err, null, null);

            return new Response(new Error(false, "Data for symbol " + arg1 + " updated between " + tFrom.ToString("yyyy-MM-dd HH:mm:ss") + " and " + tNow.ToString("yyyy-MM-dd HH:mm:ss")), null, null);
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
                return new Response(err, null, null);

            DateTime tNow = DateTime.UtcNow;
            DateTime tFrom = DateTime.Parse(arg2);
            List<Bid> bids = new List<Bid>();

            err = Xtb.Load_bids_symbol(MyDB, new Symbol(0, arg1, ""), tNow, tFrom, ref bids);
            if (err.IsAnError)
                return new Response(err, null, null);

            return new Response(new Error(false, "Data ok"), bids, null);
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
                return new Response(err, null, null);

            DateTime tNow = DateTime.UtcNow;
            DateTime tFrom = DateTime.Parse(arg1);
            List<Bid> bids = new List<Bid>();

            err = Xtb.Load_bids_symbols(MyDB, tNow, tFrom, ref bids);
            if (err.IsAnError)
                return new Response(err, null, null);

            return new Response(new Error(false, "Data ok"), bids, null);
        }

        [HttpGet]
        public Response Get_Symbols_To_Retrieve()
        {
            Error err;
            Mysql MyDB = new Mysql();
            Configuration configuration = new Configuration();
            configuration.LoadAPIConfigurationSettings();

            err = Tool.InitMyDb(ref configuration, ref MyDB);
            if (err.IsAnError)
                return new Response(err, null, null);

            List<Symbol> symbols = new List<Symbol>();

            err = Xtb.Load_symbols_to_retrieve(MyDB, ref symbols);
            if (err.IsAnError)
                return new Response(err, null, null);

            return new Response(new Error(false, "Data ok"), null, symbols);
        }
    }
}
