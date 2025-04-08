using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace ElectronicGadgets
{
    public class DuplicateProductException : Exception
    {
        public DuplicateProductException(string message) : base(message)
        {
        }
    }
    public class ProductUnavailableException : Exception
    {
        public ProductUnavailableException(string message) : base(message)
        {
        }
    }

public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(string message) : base(message) { }
}


    public class InvalidDataException : Exception
    {
        public InvalidDataException(string message) : base(message) { }              
    }
    public class InsufficientStockException : Exception 
    {
        public InsufficientStockException(string message) : base(message) { }
    }
    public class IncompleteOrderException : Exception
    {
        public IncompleteOrderException(string message) : base(message) { }
    }
    public class PaymentFailedException : Exception
    {
        public PaymentFailedException(string message) : base(message) { }
    }

}
