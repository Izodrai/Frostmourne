using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualization.Jobs.ActiveSymbols
{
    public class ActiveSymbol
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ActiveSymbol(int _id, string _name, string _description)
        {
            this.Id = _id;
            this.Name = _name;
            this.Description = _description;
        }
    }
}
