using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataRetriever.Errors;
using DataRetriever.Jobs.Bids;
using DataRetriever.Logs;

namespace DataRetriever.Jobs.Calculations
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

        public double Macd_absol_max_signal { get; set; }

        public double Macd_absol_trigger_signal { get; set; }

        public int Macd_trigger_percent { get; set; }

        public bool Data_to_update { get; set; }

        public Calculation(int _id, double _sma_c, double _sma_l, double _ema_c, double _ema_l, double _macd_value, double _macd_trigger, double _macd_signal, double _macd_absol_max_signal, double _macd_absol_trigger_signal, int _macd_trigger_percent)
        {
            this.Id = _id;
            this.Sma_c = _sma_c;
            this.Sma_l = _sma_l;
            this.Ema_c = _ema_c;
            this.Ema_l = _ema_l;
            this.Macd_value = _macd_value;
            this.Macd_trigger = _macd_trigger;
            this.Macd_signal = _macd_signal;
            this.Macd_absol_max_signal = _macd_absol_max_signal;
            this.Macd_absol_trigger_signal = _macd_absol_trigger_signal;
            this.Macd_trigger_percent = _macd_trigger_percent;
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
                range_c.Add(b.Last_bid);
                range_l.Add(b.Last_bid);

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
                if (last_value_c == 0 && last_value_l == 0)
                {
                    last_value_c = b.Last_bid;
                    last_value_l = b.Last_bid;
                }

                double v_c = last_value_c + ((b.Last_bid - last_value_c) * (2 / (c + 1)));
                double v_c_r = Math.Round(v_c, 2);
                last_value_c = v_c;
                
                if (v_c_r != b.Calculation.Ema_c)
                {
                    b.Calculation.Data_to_update = true;
                    b.Calculation.Ema_c = v_c_r;
                }
                
                double v_l = last_value_l + ((b.Last_bid - last_value_l) * (2 / (l + 1)));
                double v_l_r = Math.Round(v_l, 2);
                last_value_l = v_l;

                if (v_l_r != b.Calculation.Ema_l)
                {
                    b.Calculation.Data_to_update = true;
                    b.Calculation.Ema_l = v_l_r;
                }
            }

            return new Error(false, "EMAs calculated");
        }

        /// <summary>
        /// Calcul des indécateurs du MACD
        /// </summary>
        /// <param name="_bids_to_calculate"></param>
        /// <returns></returns>
        public static Error MACD(ref List<Bid> _bids_to_calculate, int trigger)
        {
            double d = 9;
            double last_value_d = 0.0;
            List<double> macd_signals = new List<double>();

            foreach (Bid b in _bids_to_calculate)
            {
                double v_macd = b.Calculation.Ema_c - b.Calculation.Ema_l;
                double v_macd_r = Math.Round(b.Calculation.Ema_c - b.Calculation.Ema_l, 2);
                double v_trigger = last_value_d + ((v_macd - last_value_d) * (2 / (d + 1)));
                double v_trigger_r = Math.Round(v_trigger, 2);
                last_value_d = v_trigger;

                if (v_trigger_r != b.Calculation.Macd_trigger || v_macd_r != b.Calculation.Macd_value)
                {
                    b.Calculation.Data_to_update = true;
                    b.Calculation.Macd_value = v_macd_r;
                    b.Calculation.Macd_trigger = v_trigger_r;
                    b.Calculation.Macd_signal = Math.Round(v_macd_r - v_trigger_r, 2);
                }

                macd_signals.Add(Math.Abs(b.Calculation.Macd_signal));
            }

            /*

            List<double> macd_signals = new List<double>();
            List<double> macd_signals_test = new List<double>();

            foreach (Bid b in _bids_to_calculate)
            {
                double v_macd = b.Calculation.Ema_c - b.Calculation.Ema_l;
                double v_macd_r = Math.Round(b.Calculation.Ema_c - b.Calculation.Ema_l,2);
                double v_trigger = last_value_d + ((v_macd - last_value_d) * (2 / (d + 1)));
                double v_trigger_r = Math.Round(v_trigger, 2);
                last_value_d = v_trigger;
                
                if (v_trigger_r != b.Calculation.Macd_trigger || v_macd_r != b.Calculation.Macd_value)
                {
                    b.Calculation.Data_to_update = true;
                    b.Calculation.Macd_value = v_macd_r;
                    b.Calculation.Macd_trigger = v_trigger_r;
                    b.Calculation.Macd_signal = Math.Round(v_macd_r - v_trigger_r, 2);
                }
                
                //////////////////////////////
                // 
                //////////////////////////////

                double last_absol_max_signal = 0.0;

                if (macd_signals.Count() >= 48) //288 -> 24h | 48 for 4h
                {
                    macd_signals.RemoveAt(0);
                }
                
                macd_signals.Add(Math.Abs(b.Calculation.Macd_signal));

                macd_signals_test.Add(Math.Abs(b.Calculation.Macd_signal));


                double max = 0.0;
                double max_1 = 0.0;
                double max_2 = 0.0;
                
                foreach (double signal in macd_signals_test)
                {
                    if ( signal > max )
                    {
                        max_2 = max_1;
                        max_1 = max;
                        max = signal;
                        continue;
                    }
                    if (signal > max_1)
                    {
                        max_2 = max = 1;
                        max_1 = signal;
                        continue;
                    }
                    if (signal > max_2)
                    {
                        max_2 = signal;
                    }
                }
                Console.WriteLine(max + " - " + max_1 + " - " + max_2);


                foreach (double signal in macd_signals)
                {
                    if (signal > last_absol_max_signal)
                        last_absol_max_signal = signal;
                }
                
                if (b.Calculation.Macd_absol_max_signal == 0)
                {
                    b.Calculation.Data_to_update = true;
                    b.Calculation.Macd_trigger_percent = trigger;
                    b.Calculation.Macd_absol_max_signal = last_absol_max_signal;
                    b.Calculation.Macd_absol_trigger_signal = (b.Calculation.Macd_absol_max_signal * trigger) / (double)100;
                }

                if (b.Calculation.Macd_absol_max_signal != last_absol_max_signal)
                {
                    b.Calculation.Data_to_update = true;
                    b.Calculation.Macd_absol_max_signal = last_absol_max_signal;
                    b.Calculation.Macd_absol_trigger_signal = (b.Calculation.Macd_absol_max_signal * trigger) / (double)100;
                }

            }*/

            return new Error(false, "MACD calculated");
        }
    }
}
