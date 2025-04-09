using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementSystem1
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int CreditScore { get; set; }
        public Customer()
        {
        }
        public Customer(int customerId, string name, string email, string phone, string address, int creditScore)
        {
            CustomerId = customerId;
            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
            CreditScore = creditScore;
        }
        public void PrintCustomerDetails()
        {
            Console.WriteLine("Customer ID: " + CustomerId);
            Console.WriteLine("Name: " + Name);
            Console.WriteLine("Email: " + Email);
            Console.WriteLine("Phone: " + Phone);
            Console.WriteLine("Address: " + Address);
            Console.WriteLine("Credit Score: " + CreditScore);
        }
    }

}




