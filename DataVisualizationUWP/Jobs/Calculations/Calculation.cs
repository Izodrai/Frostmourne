using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualization.Jobs.Calculations
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
    }
}
