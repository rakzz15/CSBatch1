using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementSystem1
{
    class Program
    {
        public static void Main(string[] args)
        {

            ILoanRepository repo = new LoanRepositoryImpl();

            while (true)
            {
                Console.WriteLine("\n===== Loan Management Menu =====");
                Console.WriteLine("1. Apply Loan");
                Console.WriteLine("2. Calculate Interest");
                Console.WriteLine("3. Calculate EMI");
                Console.WriteLine("4. Check Loan Status");
                Console.WriteLine("5. Repay Loan");
                Console.WriteLine("6. View All Loans");
                Console.WriteLine("7. View Loan by ID");
                Console.WriteLine("8. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ApplyLoan(repo);
                        break;
                    case "2":
                        Console.Write("Enter Loan ID: ");
                        int interestId = int.Parse(Console.ReadLine());
                        double interest = repo.CalculateInterest(interestId);
                        Console.WriteLine("Calculated Interest: " + interest);
                        break;
                    case "3":
                        Console.Write("Enter Loan ID: ");
                        int emiId = int.Parse(Console.ReadLine());
                        double emi = repo.CalculateEMI(emiId);
                        Console.WriteLine("EMI: " + emi);
                        break;
                    case "4":
                        Console.Write("Enter Loan ID: ");
                        int statusId = int.Parse(Console.ReadLine());
                        string status = repo.LoanStatus(statusId);
                        Console.WriteLine("Loan Status: " + status);
                        break;
                    case "5":
                        Console.Write("Enter Loan ID: ");
                        int repayId = int.Parse(Console.ReadLine());
                        Console.Write("Enter amount to repay: ");
                        double amount = double.Parse(Console.ReadLine());
                        repo.LoanRepayment(repayId, amount);
                        break;
                    case "6":
                        var loans = repo.GetAllLoans();
                        foreach (var loan in loans)
                        {
                            loan.PrintLoanDetails();
                            Console.WriteLine("-------------");
                        }
                        break;
                    case "7":
                        Console.Write("Enter Loan ID: ");
                        int id = int.Parse(Console.ReadLine());
                        Loan foundLoan = repo.GetLoanById(id);
                        if (foundLoan != null)
                        {
                            foundLoan.PrintLoanDetails();
                        }
                        break;
                    case "8":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        private static void ApplyLoan(ILoanRepository repo)
        {
            Console.WriteLine("Enter Loan Details");

            Console.Write("Loan ID: ");
            int loanId = int.Parse(Console.ReadLine());

            Console.Write("Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());

            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Phone: ");
            string phone = Console.ReadLine();

            Console.Write("Address: ");
            string address = Console.ReadLine();

            Console.Write("Credit Score: ");
            int creditScore = int.Parse(Console.ReadLine());

            Customer customer = new Customer(customerId, name, email, phone, address, creditScore);

            Console.Write("Principal Amount: ");
            double principal = double.Parse(Console.ReadLine());

            Console.Write("Interest Rate (%): ");
            double interestRate = double.Parse(Console.ReadLine());

            Console.Write("Loan Term (in months): ");
            int loanTerm = int.Parse(Console.ReadLine());

            Console.Write("Loan Type (HomeLoan/CarLoan): ");
            string loanType = Console.ReadLine();

            Loan loan = null;

            if (loanType.Equals("HomeLoan", StringComparison.OrdinalIgnoreCase))
            {
                Console.Write("Property Address: ");
                string propertyAddress = Console.ReadLine();

                Console.Write("Property Value: ");
                int propertyValue = int.Parse(Console.ReadLine());

                loan = new HomeLoan(loanId, customer, principal, interestRate, loanTerm, "Pending", propertyAddress, propertyValue);
            }
            else if (loanType.Equals("CarLoan", StringComparison.OrdinalIgnoreCase))
            {
                Console.Write("Car Model: ");
                string carModel = Console.ReadLine();

                Console.Write("Car Value: ");
                int carValue = int.Parse(Console.ReadLine());

                loan = new CarLoan(loanId, customer, principal, interestRate, loanTerm, "Pending", carModel, carValue);
            }
            else
            {
                Console.WriteLine("Invalid loan type entered.");
                return;
            }

            repo.ApplyLoan(loan);
        }
    }
}
    