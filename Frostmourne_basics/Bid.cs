using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frostmourne_basics
{
    public class Bid
    {
        public int Id { get; set; }

        public int Symbol_id { get; set; }

        public string Symbol_name { get; set; }

        public DateTime Bid_at { get; set; }

        public double Last_bid { get; set; }

        public Bid() { }

        public Bid(int _symbol_id, DateTime _bid_at, double _last_bid)
        {
            this.Symbol_id = _symbol_id;
            this.Bid_at = _bid_at;
            this.Last_bid = _last_bid;
        }

        public Bid(int _symbol_id, string _symbol_name, DateTime _bid_at, double _last_bid)
        {
            this.Symbol_id = _symbol_id;
            this.Symbol_name = _symbol_name;
            this.Bid_at = _bid_at;
            this.Last_bid = _last_bid;
        }
    }
}
