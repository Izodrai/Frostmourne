using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataRetriever.Jobs.Calculations;

namespace DataRetriever.Jobs.Bids
{
    class Bid
    {
        public int Id { get; set; }

        public int Symbol_id { get; set; }

        public string Symbol_name { get; set; }

        public DateTime Bid_at { get; set; }

        public double Start_bid { get; set; }

        public double Last_bid { get; set; }

        public Calculation Calculation { get; set; }

        public Bid() { }

        public Bid(int _symbol_id, DateTime _bid_at, double _start_bid, double _last_bid)
        {
            this.Symbol_id = _symbol_id;
            this.Bid_at = _bid_at;
            this.Start_bid = _start_bid;
            this.Last_bid = _last_bid;
        }

        public Bid(int _id, int _symbol_id, string _symbol_name, DateTime _bid_at, double _start_bid, double _last_bid)
        {
            this.Id = _id;
            this.Symbol_id = _symbol_id;
            this.Symbol_name = _symbol_name;
            this.Bid_at = _bid_at;
            this.Start_bid = _start_bid;
            this.Last_bid = _last_bid;
        }
        
        public Bid(int _id, int _symbol_id, string _symbol_name, DateTime _bid_at, double _start_bid, double _last_bid, double _sma_c, double _sma_l, double _ema_c, double _ema_l, int _sa_id, double _macd_value, double _macd_trigger, double _macd_signal, double _macd_absol_max_signal, double _macd_absol_trigger_signal, int _macd_trigger_percent)
        {
            this.Id = _id;
            this.Symbol_id = _symbol_id;
            this.Symbol_name = _symbol_name;
            this.Bid_at = _bid_at;
            this.Start_bid = _start_bid;
            this.Last_bid = _last_bid;
            this.Calculation = new Calculation(_sa_id, _sma_c, _sma_l, _ema_c, _ema_l, _macd_value, _macd_trigger, _macd_signal, _macd_absol_max_signal, _macd_absol_trigger_signal, _macd_trigger_percent);
        }
    }
}
