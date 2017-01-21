﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XtbDataRetriever.Errors;
using XtbDataRetriever.Jobs.Bids;
using XtbDataRetriever.Logs;

namespace XtbDataRetriever.Jobs.Calculations
{
    class Calculation
    {
        public int Id { get; set; }

        public double Sma_c { get; set; }

        public double Sma_l { get; set; }

        public double Ema_c { get; set; }

        public double Ema_l { get; set; }

        public double Macd_value { get; set; }

        public double Macd_trigger { get; set; }

        public double Macd_signal { get; set; }

        public bool Data_to_update { get; set; }

        public Calculation(int _id, double _sma_c, double _sma_l, double _ema_c, double _ema_l)
        {
            this.Id = _id;
            this.Sma_c = _sma_c;
            this.Sma_l = _sma_l;
            this.Ema_c = _ema_c;
            this.Ema_l = _ema_l;
        }

        /// <summary>
        /// Calcul des moyennes mobiles simple
        /// </summary>
        /// <param name="_bids_to_calculate"></param>
        /// <returns></returns>
        public static Error SMA(ref List<Bid> _bids_to_calculate)
        {
            double c = 12;
            double l = 24;

            List<double> range_c = new List<double>();
            List<double> range_l = new List<double>();

            foreach (Bid b in _bids_to_calculate)
            {
                range_c.Add(b.Last_bid_value);
                range_l.Add(b.Last_bid_value);

                if (range_c.Count() > c)
                    range_c.RemoveAt(0);

                if (range_l.Count() > l)
                    range_l.RemoveAt(0);

                double sma_c = 0;
                foreach (double d in range_c)
                {
                    sma_c = sma_c + d;
                }

                sma_c = Math.Round(sma_c / c, 0);

                if (sma_c != b.Calculation.Sma_c)
                {
                    b.Calculation.Data_to_update = true;
                    b.Calculation.Sma_c = sma_c;
                }
                
                double sma_l = 0;          
                foreach (double d in range_l)
                {
                    sma_l = sma_l + d;
                }
                sma_l = Math.Round(sma_l / l, 0);

                if (sma_l != b.Calculation.Sma_l)
                {
                    b.Calculation.Data_to_update = true;
                    b.Calculation.Sma_l = sma_l;
                }
            }
            
            return new Error(false, "SMAs calculated");
        }

        /// <summary>
        /// Calcul des moyennes mobiles exponentielles
        /// </summary>
        /// <param name="_bids_to_calculate"></param>
        /// <returns></returns>
        public static Error EMA(ref List<Bid> _bids_to_calculate)
        {
            double c = 12;
            double l = 24;

            double last_value_c = 0;
            double last_value_l = 0;
            
            foreach (Bid b in _bids_to_calculate)
            {
                
                double v_c = last_value_c + ((b.Last_bid_value - last_value_c) * (2 / (c + 1)));
                double v_c_r = Math.Round(v_c, 0);
                last_value_c = v_c;
                
                if (v_c_r != b.Calculation.Ema_c)
                {
                    b.Calculation.Data_to_update = true;
                    b.Calculation.Ema_c = v_c_r;
                }
                
                double v_l = last_value_l + ((b.Last_bid_value - last_value_l) * (2 / (l + 1)));
                double v_l_r = Math.Round(v_l, 0);
                last_value_l = v_l;

                if (v_l_r != b.Calculation.Ema_l)
                {
                    b.Calculation.Data_to_update = true;
                    b.Calculation.Ema_l = v_l_r;
                }
            }

            return new Error(false, "MACD calculated");
        }
    }
}
