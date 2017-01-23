﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XtbDataRetriever.Jobs.Calculations;

namespace XtbDataRetriever.Jobs.Bids
{
    class Bid
    {
        public int Id { get; set; }

        public int Symbol_id { get; set; }

        public string Symbol_name { get; set; }

        public DateTime Bid_at { get; set; }

        public double Start_bid_value { get; set; }

        public double Last_bid_value { get; set; }

        public Calculation Calculation { get; set; }

        public Bid() { }

        public Bid(int _symbol_id, DateTime _bid_at, double _start_bid_value, double _last_bid_value)
        {
            this.Symbol_id = _symbol_id;
            this.Bid_at = _bid_at;
            this.Start_bid_value = _start_bid_value;
            this.Last_bid_value = _last_bid_value;
        }

        public Bid(int _id, int _symbol_id, string _symbol_name, DateTime _bid_at, double _start_bid_value, double _last_bid_value)
        {
            this.Id = _id;
            this.Symbol_id = _symbol_id;
            this.Symbol_name = _symbol_name;
            this.Bid_at = _bid_at;
            this.Start_bid_value = _start_bid_value;
            this.Last_bid_value = _last_bid_value;
        }
        
        public Bid(int _id, int _symbol_id, string _symbol_name, DateTime _bid_at, double _start_bid_value, double _last_bid_value, double _sma_c, double _sma_l, double _ema_c, double _ema_l, int _sa_id, double _macd_value, double _macd_trigger, double _macd_signal)
        {
            this.Id = _id;
            this.Symbol_id = _symbol_id;
            this.Symbol_name = _symbol_name;
            this.Bid_at = _bid_at;
            this.Start_bid_value = _start_bid_value;
            this.Last_bid_value = _last_bid_value;
            this.Calculation = new Calculation(_sa_id, _sma_c, _sma_l, _ema_c, _ema_l, _macd_value, _macd_trigger, _macd_signal);
        }
    }
}
