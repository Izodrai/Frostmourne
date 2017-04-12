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

        public Symbol Symbol { get; set; }

        public DateTime Bid_at { get; set; }

        public double Last_bid { get; set; }

        public string Calculations { get; set; }

        public bool To_add_or_update { get; set; }

        public Bid() { }

        public Bid(Symbol _symbol, DateTime _bid_at, double _last_bid, string _calculations, bool _to_add_or_update)
        {
            this.Symbol = _symbol;
            this.Bid_at = _bid_at;
            this.Last_bid = _last_bid;

            if (_calculations == "")
                _calculations = "{}";

            this.Calculations = _calculations;

            this.To_add_or_update = _to_add_or_update;
        }

        public Bid(int _id, Symbol _symbol, DateTime _bid_at, double _last_bid, string _calculations, bool _to_add_or_update)
        {
            this.Id = _id;
            this.Symbol = _symbol;
            this.Bid_at = _bid_at;
            this.Last_bid = _last_bid;

            if (_calculations == "")
                _calculations = "{}";

            this.Calculations = _calculations;

            this.To_add_or_update = _to_add_or_update;
        }
    }
}
