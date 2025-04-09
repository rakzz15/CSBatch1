using System;


namespace LoanManagementSystem.Exceptions
{
    public class InvalidLoanException : ApplicationException
    {
        public InvalidLoanException() : base("Invalid Loan") { }

        public InvalidLoanException(string message) : base(message) { }

        public InvalidLoanException(string message, Exception inner)
            : base(message, inner) { }
    }
}
