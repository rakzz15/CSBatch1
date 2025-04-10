using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportManagementSystem.Entity
{
    public class Vehicle
    {
        public int VehicleID { get; set; }
        public string Model { get; set; }
        public double Capacity { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}
