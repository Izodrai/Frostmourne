using DataAPI.Models;
using Frostmourne_basics;
using Frostmourne_basics.Commands;
using Frostmourne_basics.Dbs;
using System;
using System.Collections.Generic;
using System.Web.Http;
using xAPI.Sync;

namespace DataAPI.Controllers
{
    public partial class BidsController : ApiController
    {
        [HttpGet]
        public Response Feed_Symbol_From_Last_Insert(string arg1)
        {
            Error err;
            Mysql MyDB = new Mysql();
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration();
            Data_api_configuration.LoadAPIConfigurationSettings(ref configuration);

            err = Tool.InitAll(ref Xtb_api_connector, ref configuration, ref MyDB);
            if (err.IsAnError)
                return new Response(err, null, null);

            List<Symbol> symbol_list = new List<Symbol>();

            err = Commands.Load_all_symbols_status(ref configuration, ref MyDB, ref symbol_list);
            if (err.IsAnError)
                return new Response(err, null, null);

            Symbol s_to_feed = new Symbol();

            foreach (Symbol s in symbol_list)
            {
                if (s.Id.ToString() == arg1)
                {
                    s_to_feed = s;
                    break;
                }
            }

            if (s_to_feed.Id == 0)
                return new Response(new Error(false, "This ID doesn't exist or inactive : " + arg1), null, null);

            s_to_feed.Description = "";

            List<Bid> bids_treated = new List<Bid>();
            err = Commands.Get_from_xtb_stock_values_from_last_insert_for_symbol(ref Xtb_api_connector, ref configuration, ref MyDB, s_to_feed, ref bids_treated);
            if (err.IsAnError)
                return new Response(err, null, null);

            return new Response(new Error(false, "Data for symbol " + s_to_feed.Id.ToString() + " (" + s_to_feed.Name + ") feeded"), bids_treated, null);
        }

        [HttpGet]
        public Response Get_Data_For_Symbol(string arg1, string arg2, string arg3)
        {
            Error err;
            Mysql MyDB = new Mysql();
            Configuration configuration = new Configuration();
            Data_api_configuration.LoadAPIConfigurationSettings(ref configuration);

            err = Tool.InitMyDb(ref configuration, ref MyDB);
            if (err.IsAnError)
                return new Response(err, null, null);

            List<Symbol> symbol_list = new List<Symbol>();

            err = Commands.Load_all_symbols_status(ref configuration, ref MyDB, ref symbol_list);
            if (err.IsAnError)
                return new Response(err, null, null);

            Symbol s_to_load = new Symbol();

            foreach (Symbol s in symbol_list)
            {
                if (s.Id.ToString() == arg1)
                {
                    s_to_load = s;
                    break;
                }
            }

            if (s_to_load.Id == 0)
                return new Response(new Error(false, "This ID doesn't exist or inactive : " + arg1), null, null);

            DateTime from = new DateTime();

            try
            {
                from = DateTime.Parse(arg2);
            }
            catch
            {
                return new Response(new Error(false, "Bad date format -> " + arg2), null, null);
            }

            if (from == new DateTime())
            {
                return new Response(new Error(false, "Bad date format -> " + arg2), null, null);
            }

            DateTime to = new DateTime();

            try
            {
                to = DateTime.Parse(arg3);
            }
            catch
            {
                return new Response(new Error(false, "Bad date format -> " + arg3), null, null);
            }

            if (to == new DateTime())
            {
                return new Response(new Error(false, "Bad date format -> " + arg3), null, null);
            }

            if (from.Date >= to.Date)
            {
                return new Response(new Error(false, "from.Date >= to.Date"), null, null);
            }

            List<Bid> bids = new List<Bid>();

            err = Commands.Get_from_db_stock_values_between_two_date_for_symbol(ref configuration, ref MyDB, s_to_load, from, to, ref bids);
            if (err.IsAnError)
                return new Response(err, null, null);

            return new Response(new Error(false, "Data for symbol " + s_to_load.Id.ToString() + " (" + s_to_load.Name + ") loaded"), bids, null);
        }

        [HttpGet]
        public Response Set_Calculation(string arg1, string arg2)
        {
            Error err;
            Mysql MyDB = new Mysql();
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration();
            Data_api_configuration.LoadAPIConfigurationSettings(ref configuration);

            err = Tool.InitAll(ref Xtb_api_connector, ref configuration, ref MyDB);
            if (err.IsAnError)
                return new Response(err, null, null);
            
            Bid bid_to_update = new Bid();
            List<Bid> bids_to_update = new List<Bid>();

            bid_to_update.Id = Convert.ToInt32(arg1);
            bid_to_update.Calculations = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(arg2));

            bids_to_update.Add(bid_to_update);

            err = Commands.Update_db_stock_values_calculation(ref configuration, ref MyDB, ref bids_to_update);
            if (err.IsAnError)
                return new Response(err, null, null);
            
            return new Response(new Error(false, "This bid calculation has been updated"), bids_to_update, null);
        }
    }
}
