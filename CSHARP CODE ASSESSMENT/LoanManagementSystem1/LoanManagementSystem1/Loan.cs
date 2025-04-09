using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementSystem1
{
    public class Loan
    {
            public int LoanId { get; set; }
            public Customer Customer { get; set; }
            public double PrincipalAmount { get; set; }
            public double InterestRate { get; set; }
            public int LoanTerm { get; set; }
            public string LoanType { get; set; }
            public string LoanStatus { get; set; }

            public Loan()
            {
            }
            public Loan(int loanId, Customer customer, double principalAmount, double interestRate, int loanTerm, string loanType, string loanStatus)
            {
                LoanId = loanId;
                Customer = customer;
                PrincipalAmount = principalAmount;
                InterestRate = interestRate;
                LoanTerm = loanTerm;
                LoanType = loanType;
                LoanStatus = loanStatus;
            }
            public virtual void PrintLoanDetails()
            {
                Console.WriteLine("Loan ID: " + LoanId);
                Console.WriteLine("Customer ID: " + Customer?.CustomerId);
                Console.WriteLine("Principal Amount: " + PrincipalAmount);
                Console.WriteLine("Interest Rate: " + InterestRate);
                Console.WriteLine("Loan Term (months): " + LoanTerm);
                Console.WriteLine("Loan Type: " + LoanType);
                Console.WriteLine("Loan Status: " + LoanStatus);
            }
        }

        public class HomeLoan : Loan
        {
            public string PropertyAddress { get; set; }
            public int PropertyValue { get; set; }
            public HomeLoan() : base() { }
            public HomeLoan(int loanId, Customer customer, double principalAmount, double interestRate, int loanTerm, string loanStatus, string propertyAddress, int propertyValue) : base(loanId, customer, principalAmount, interestRate, loanTerm, "HomeLoan", loanStatus)
            {
                PropertyAddress = propertyAddress;
                PropertyValue = propertyValue;
            }
            public override void PrintLoanDetails()
            {
                base.PrintLoanDetails();
                Console.WriteLine("Property Address: " + PropertyAddress);
                Console.WriteLine("Property Value: " + PropertyValue);
            }
        }

        public class CarLoan : Loan
        {
            public string CarModel { get; set; }
            public int CarValue { get; set; }
            public CarLoan() : base() { }
            public CarLoan(int loanId, Customer customer, double principalAmount, double interestRate, int loanTerm, string loanStatus, string carModel, int carValue) : base(loanId, customer, principalAmount, interestRate, loanTerm, "CarLoan", loanStatus)
            {
                CarModel = carModel;
                CarValue = carValue;
            }
            public override void PrintLoanDetails()
            {
                base.PrintLoanDetails();
                Console.WriteLine("Car Model: " + CarModel);
                Console.WriteLine("Car Value: " + CarValue);
            }
        }
    }

