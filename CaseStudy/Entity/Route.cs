using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportManagementSystem.Entity
{
    public class Route
    {
        public int RouteID { get; set; }
        public string StartDestination { get; set; }
        public string EndDestination { get; set; }
        public double Distance { get; set; }

    }
}
