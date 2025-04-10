using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportManagementSystem.Entity
{
    public class Booking
    {
        public int BookingID { get; set; }
        public int TripID { get; set; }
        public int PassengerID { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; }
    }
}
