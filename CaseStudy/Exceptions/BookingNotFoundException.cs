using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportManagementSystem.Exceptions
{
    public class BookingNotFoundException : Exception
    {
        public BookingNotFoundException(string message) : base(message) { }
    }
}
