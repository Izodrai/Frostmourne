using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XtbDataRetriever.Errors;
using XtbDataRetriever.Jobs.Bids;

namespace XtbDataRetriever.Jobs.Calculations
{
    class Calculation
    {
        public double Mm_c { get; set; }

        public double Mm_l { get; set; }

        public double Mme_c { get; set; }

        public double Mme_l { get; set; }

        public double Macd_value { get; set; }

        public double Macd_trigger { get; set; }

        public double Macd_signal { get; set; }


        public static Error MM(ref List<Bid> _last_bids)
        {
            foreach (Bid b in _last_bids)
            {
                b.Calculation.Mme_c = 0;
                b.Calculation.Mme_l = 0;
            }

             return new Error(false, "work in progress");
        }

        public Error Macd(ref List<Bid> _last_bids)
        {

            return new Error(false, "work in progress");
        }
    }
}
