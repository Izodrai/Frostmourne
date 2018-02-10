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
        public Response Get_Xtb_Bids(string xtb_id, string xtb_log, string xtb_env, string arg1, string arg2)
        {
            Error err;
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration(xtb_id, xtb_log, xtb_env);
            
            DateTime now = DateTime.Now;
            TimeSpan tsFrom = TimeSpan.Zero;

            try
            {
                tsFrom = TimeSpan.FromSeconds(Convert.ToInt64(arg2));
            }
            catch
            {
                return new Response(new Error(false, "Bad timespan format 1 -> " + arg2), null, null, null);
            }

            if (tsFrom == TimeSpan.Zero)
                return new Response(new Error(false, "Bad timespan format 2 -> " + arg2), null, null, null);
            
            DateTime from = new DateTime(1970, 1, 1).Add(tsFrom);
            tsFrom = tsFrom.Subtract(new TimeSpan(0, 0, 1));
            
            if (from.Date > now.Date)
                return new Response(new Error(false, "from.Date > now.Date"), null, null, null);
            
            List<Bid> bids = new List<Bid>();
            err = Commands.Get_stock_values_from_xtb(ref Xtb_api_connector, ref configuration, arg1, ref bids, Convert.ToInt64(arg2));
            if (err.IsAnError)
                return new Response(err, null, null, null);
                
            return new Response(new Error(false, "Data for symbol " + arg1 + " retrieved from " + from + " to " + now), bids, null, null);
        }
        
        [HttpGet]
        public Response Get_Open_Trades(string xtb_id, string xtb_log, string xtb_env)
        {
            Error err;
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration(xtb_id, xtb_log, xtb_env);

            List<Trade> trades = new List<Trade>();
            
            err = Commands.Get_open_trades_from_xtb(ref Xtb_api_connector, ref configuration, ref trades);
            if (err.IsAnError)
                return new Response(err, null, null, null);

            return new Response(new Error(false, ""), null, null, trades);
        }
        
        [HttpGet]
        public Response Open_Trade(string xtb_id, string xtb_log, string xtb_env, string arg1, string arg2, string arg3, string arg4)
        {
            Error err;
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration(xtb_id, xtb_log, xtb_env);
            
            List<Trade> trades = new List<Trade>();
            
            Symbol symbol = new Symbol();
            Trade trade = new Trade();

            try
            {
                symbol.Name = arg1;
                trade.Symbol = symbol;
                trade.Trade_type = Convert.ToInt32(arg2);
                trade.Volume = Convert.ToDouble(arg3);
                trade.Opened_reason = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(arg4));
            }
            catch (Exception e)
            {
                return new Response(new Error(true, e.Message), null, null, null);
            }
            
            err = Commands.Open_trade_xtb(ref Xtb_api_connector, ref configuration, symbol, ref trade);
            if (err.IsAnError)
                return new Response(err, null, null, null);
            
            trades.Add(trade);

            return new Response(new Error(false, ""), null, null, trades);
        }
        
        [HttpGet]
        public Response Close_Trade(string xtb_id, string xtb_log, string xtb_env, string arg1, string arg2, string arg3)
        {
            Error err;
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration(xtb_id, xtb_log, xtb_env);

            Trade trade_to_close = new Trade();
            Symbol symbol = new Symbol();

            try
            {
                symbol.Name = arg1;
                trade_to_close.Symbol = symbol;
                trade_to_close.Xtb_order_id_1 = Convert.ToInt32(arg2);
                trade_to_close.Closed_reason = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(arg3));
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
        
    }
}
