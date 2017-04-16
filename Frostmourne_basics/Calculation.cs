using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frostmourne_basics
{
    public class Calculation
    {
        public string Type { get; set; }
        public double Value { get; set; }

        public Calculation () {}

        public Calculation (string _type, double _value)
        {
            this.Type = _type;
            this.Value = _value;
        }
    }
}
