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

        public void Calc_bid(ref List<Bid> last_bids, Configuration calc_config)
        {
            List<Calculation> calculations = new List<Calculation>{};

            foreach (int ct_period in calc_config.SMA_values)
            {
                double sma = 0;
                Calculation c = new Calculation();

                calc_sma(ref last_bids, ct_period,ref c);
                Log.Info(ct_period.ToString() + " - " + sma.ToString());

                calculations.Add(c);
            }
        }

        protected void calc_sma(ref List<Bid> last_bids, int ct_period, ref Calculation sma)
        {
            if (last_bids.Count < ct_period)
                return;

            double s_sma = this.Last_bid;

            for (int i = last_bids.Count - 1; i >= last_bids.Count - ct_period + 1; --i)
            {
                Bid bid = last_bids[i];
                s_sma += bid.Last_bid;
            }

            sma.Type = "sma_" + ct_period.ToString();
            sma.Value = Math.Round((s_sma / ct_period),2);
        }
    }
}
