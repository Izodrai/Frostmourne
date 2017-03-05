using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualization.Jobs.Symbols
{
    public class Symbol
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Symbol(int _id, string _name, string _description)
        {
            this.Id = _id;
            this.Name = _name;
            this.Description = _description;
        }
    }
}
