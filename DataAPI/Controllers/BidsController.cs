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
        public Response Get_Xtb_Bids(string arg1, string arg2, string arg3)
        {
            Error err;
            Mysql MyDB = new Mysql();
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration();
            Data_api_configuration.LoadAPIConfigurationSettings(ref configuration);

            if (!Token_validation.TokenValid(ref configuration, arg1))
                return new Response(new Error(true,"bad token"), null, null, null);
            
            DateTime from = new DateTime();
            DateTime now = DateTime.Now;
            
            try
            {
                string d = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(arg3));
                from = DateTime.Parse(d);
            }
            catch
            {
                return new Response(new Error(false, "Bad date format -> " + arg3), null, null, null);
            }

            if (from == new DateTime())
            {
                return new Response(new Error(false, "Bad date format -> " + arg3), null, null, null);
            }

            if (from.Date > now.Date)
            {
                return new Response(new Error(false, "from.Date > now.Date"), null, null, null);
            }
            
            List<Bid> bids = new List<Bid>();
            err = Commands.Get_stock_values_from_xtb(ref Xtb_api_connector, ref configuration, ref MyDB, arg2, ref bids, from);
            if (err.IsAnError)
                return new Response(err, null, null, null);
            
            return new Response(new Error(false, "Data for symbol " + arg2 + " retrieved"), bids, null, null);
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
