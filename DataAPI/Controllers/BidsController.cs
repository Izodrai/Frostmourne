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
                return new Response(err, null, null, null);
            
            Symbol s_to_feed = new Symbol();

            s_to_feed.Id = Convert.ToInt32(arg1);
            s_to_feed.Description = "";

            List<Bid> bids_treated = new List<Bid>();
            err = Commands.Get_from_xtb_stock_values_from_last_insert_for_symbol(ref Xtb_api_connector, ref configuration, ref MyDB, s_to_feed, ref bids_treated);
            if (err.IsAnError)
                return new Response(err, null, null, null);

            return new Response(new Error(false, "Data for symbol " + s_to_feed.Id.ToString() + " (" + s_to_feed.Name + ") feeded"), bids_treated, null, null);
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
                return new Response(err, null, null, null);

            Symbol s_to_load = new Symbol();

            s_to_load.Id = Convert.ToInt32(arg1);

            DateTime from = new DateTime();

            try
            {
                from = DateTime.Parse(arg2);
            }
            catch
            {
                return new Response(new Error(false, "Bad date format -> " + arg2), null, null, null);
            }

            if (from == new DateTime())
            {
                return new Response(new Error(false, "Bad date format -> " + arg2), null, null, null);
            }

            DateTime to = new DateTime();

            try
            {
                to = DateTime.Parse(arg3);
            }
            catch
            {
                return new Response(new Error(false, "Bad date format -> " + arg3), null, null, null);
            }

            if (to == new DateTime())
            {
                return new Response(new Error(false, "Bad date format -> " + arg3), null, null, null);
            }

            if (from.Date >= to.Date)
            {
                return new Response(new Error(false, "from.Date >= to.Date"), null, null, null);
            }

            List<Bid> bids = new List<Bid>();

            err = Commands.Get_from_db_stock_values_between_two_date_for_symbol(ref configuration, ref MyDB, s_to_load, from, to, ref bids);
            if (err.IsAnError)
                return new Response(err, null, null, null);

            return new Response(new Error(false, "Data for symbol " + s_to_load.Id.ToString() + " (" + s_to_load.Name + ") loaded"), bids, null, null);
        }

        [HttpGet]
        public Response Set_Calculation(string arg1, string arg2)
        {
            Error err;
            Mysql MyDB = new Mysql();
            Configuration configuration = new Configuration();
            Data_api_configuration.LoadAPIConfigurationSettings(ref configuration);

            err = Tool.InitMyDb(ref configuration, ref MyDB);
            if (err.IsAnError)
                return new Response(err, null, null, null);
            
            Bid bid_to_update = new Bid();
            List<Bid> bids_to_update = new List<Bid>();

            bid_to_update.Id = Convert.ToInt32(arg1);
            bid_to_update.Calculations = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(arg2));

            bids_to_update.Add(bid_to_update);

            err = Commands.Update_db_stock_values_calculation(ref configuration, ref MyDB, ref bids_to_update);
            if (err.IsAnError)
                return new Response(err, null, null, null);
            
            return new Response(new Error(false, "This bid calculation has been updated"), bids_to_update, null, null);
        }
        
        [HttpGet]
        public Response Get_Open_Trades()
        {
            Error err;
            Mysql MyDB = new Mysql();
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration();
            Data_api_configuration.LoadAPIConfigurationSettings(ref configuration);
            List<Trade> trades = new List<Trade>();

            err = Tool.InitAll(ref Xtb_api_connector, ref configuration, ref MyDB);
            if (err.IsAnError)
                return new Response(err, null, null, null);

            err = Commands.Get_open_trades_from_xtb(ref Xtb_api_connector, ref configuration, ref MyDB, ref trades);
            if (err.IsAnError)
                return new Response(err, null, null, null);

            return new Response(new Error(false, ""), null, null, trades);
        }

        [HttpGet]
        public Response Open_Trade(string arg1, string arg2, string arg3, string arg4)
        {
            Error err;
            Mysql MyDB = new Mysql();
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration();
            Data_api_configuration.LoadAPIConfigurationSettings(ref configuration);
            List<Trade> trades = new List<Trade>();

            err = Tool.InitAll(ref Xtb_api_connector, ref configuration, ref MyDB);
            if (err.IsAnError)
                return new Response(err, null, null, null);

            Symbol symbol = new Symbol();
            Trade trade = new Trade();

            try
            {
                symbol.Id = Convert.ToInt32(arg1);
                trade.Trade_type = Convert.ToInt32(arg2);

                trade.Volume = Convert.ToDouble(arg3);

                trade.Opened_reason = arg4;
            }
            catch (Exception e)
            {
                return new Response(new Error(true, e.Message), null, null, null);
            }
            
            err = Commands.Open_trade_xtb(ref Xtb_api_connector, ref configuration, ref MyDB, symbol, ref trade);
            if (err.IsAnError)
                return new Response(err, null, null, null);

            trades.Add(trade);

            return new Response(new Error(false, ""), null, null, trades);
        }

        [HttpGet]
        public Response Close_Trade(string arg1, string arg2)
        {
            Error err;
            Mysql MyDB = new Mysql();
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration();
            Data_api_configuration.LoadAPIConfigurationSettings(ref configuration);

            err = Tool.InitAll(ref Xtb_api_connector, ref configuration, ref MyDB);
            if (err.IsAnError)
                return new Response(err, null, null, null);
            
            Trade trade_to_close = new Trade();

            trade_to_close.Id = Convert.ToInt32(arg1);
            trade_to_close.Closed_reason = arg2;

            err = Commands.Close_trade_xtb(ref Xtb_api_connector, ref configuration, ref MyDB, ref trade_to_close);
            if (err.IsAnError)
                return new Response(err, null, null, null);

            List<Trade> trades = new List<Trade>();

            err = Commands.Get_open_trades_from_xtb(ref Xtb_api_connector, ref configuration, ref MyDB, ref trades);
            if (err.IsAnError)
                return new Response(err, null, null, null);

            return new Response(new Error(false, ""), null, null, trades);
        }
    }
}
