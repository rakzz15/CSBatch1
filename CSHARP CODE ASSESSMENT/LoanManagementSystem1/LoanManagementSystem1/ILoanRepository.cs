using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementSystem1
{
    public interface ILoanRepository
    {
        void ApplyLoan(Loan loan);
        double CalculateInterest(int loanId);
        double CalculateInterest(double principal, double rate, int months);
        string LoanStatus(int loanId);
        double CalculateEMI(int loanId);
        double CalculateEMI(double principal, double rate, int months);
        void LoanRepayment(int loanId, double amount);
        List<Loan> GetAllLoans();
        Loan GetLoanById(int loanId);

    }
}
