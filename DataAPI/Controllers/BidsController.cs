using DataAPI.Models;
using Frostmourne_basics;
using Frostmourne_basics.Commands;
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
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration();
            Data_api_configuration.LoadAPIConfigurationSettings(ref configuration);

            if (!Token_validation.TokenValid(ref configuration, arg1))
                return new Response(new Error(true, "bad token"), null, null, null);

            DateTime now = DateTime.Now;
            TimeSpan tsFrom = TimeSpan.Zero;

            try
            {
                tsFrom = TimeSpan.FromSeconds(Convert.ToInt64(arg3));
            }
            catch
            {
                return new Response(new Error(false, "Bad timespan format 1 -> " + arg3), null, null, null);
            }

            if (tsFrom == TimeSpan.Zero)
                return new Response(new Error(false, "Bad timespan format 2 -> " + arg3), null, null, null);
            
            DateTime from = new DateTime(1970, 1, 1).Add(tsFrom);
            tsFrom = tsFrom.Subtract(new TimeSpan(0, 0, 1));
            
            if (from.Date > now.Date)
                return new Response(new Error(false, "from.Date > now.Date"), null, null, null);
            
            List<Bid> bids = new List<Bid>();
            err = Commands.Get_stock_values_from_xtb(ref Xtb_api_connector, ref configuration, arg2, ref bids, Convert.ToInt64(arg3));
            if (err.IsAnError)
                return new Response(err, null, null, null);

            return new Response(new Error(false, "Data for symbol " + arg2 + " retrieved from " + from + " to " + now), bids, null, null);
        }
        /*
        [HttpGet]
        public Response Get_Open_Trades(string arg1)
        {
            Error err;
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration();
            Data_api_configuration.LoadAPIConfigurationSettings(ref configuration);

            if (!Token_validation.TokenValid(ref configuration, arg1))
                return new Response(new Error(true, "bad token"), null, null, null);

            List<Trade> trades = new List<Trade>();
            
            err = Commands.Get_open_trades_from_xtb(ref Xtb_api_connector, ref configuration, ref trades);
            if (err.IsAnError)
                return new Response(err, null, null, null);

            return new Response(new Error(false, ""), null, null, trades);
        }

        [HttpGet]
        public Response Open_Trade(string arg1, string arg2, string arg3, string arg4, string arg5)
        {
            Error err;
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration();
            Data_api_configuration.LoadAPIConfigurationSettings(ref configuration);

            if (!Token_validation.TokenValid(ref configuration, arg1))
                return new Response(new Error(true, "bad token"), null, null, null);

            
            List<Trade> trades = new List<Trade>();
            
            Symbol symbol = new Symbol();
            Trade trade = new Trade();

            try
            {
                symbol.Name = arg2;
                trade.Symbol = symbol;
                trade.Trade_type = Convert.ToInt32(arg3);
                trade.Volume = Convert.ToDouble(arg4);
                trade.Opened_reason = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(arg5));
            }
            catch (Exception e)
            {
                return new Response(new Error(true, e.Message), null, null, null);
            }
            
            err = Commands.Open_trade_xtb(ref Xtb_api_connector, ref configuration, arg2, ref trade);
            if (err.IsAnError)
                return new Response(err, null, null, null);
            
            trades.Add(trade);

            return new Response(new Error(false, ""), null, null, trades);
        }

        [HttpGet]
        public Response Close_Trade(string arg1, string arg2, string arg3, string arg4)
        {
            Error err;
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration();
            Data_api_configuration.LoadAPIConfigurationSettings(ref configuration);

            if (!Token_validation.TokenValid(ref configuration, arg1))
                return new Response(new Error(true, "bad token"), null, null, null);

            Trade trade_to_close = new Trade();
            Symbol symbol = new Symbol();

            try
            {
                symbol.Name = arg3;
                trade_to_close.Symbol = symbol;
                trade_to_close.Xtb_order_id_1 = Convert.ToInt32(arg2);
                trade_to_close.Closed_reason = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(arg4));
            }
            catch (Exception e)
            {
                return new Response(new Error(true, e.Message), null, null, null);
            }

            List<Trade> trades = new List<Trade>();

            err = Commands.Close_trade_xtb(ref Xtb_api_connector, ref configuration, ref trade_to_close);
            if (err.IsAnError)
                return new Response(err, null, null, null);
            
            trades.Add(trade_to_close);

            return new Response(new Error(false, ""), null, null, trades);
        }
        */
    }
}
