using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportManagementSystem.Entity
{
    public class Driver
    {
        public int DriverID { get; set; }
        public string Name { get; set; }
        public string LicenseNumber { get; set; }
        public string Status { get; set; }
    }
}
