using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagementSystem.Exceptions;
using LoanManagementSystem1.Util;

namespace LoanManagementSystem1
{
    public class LoanRepositoryImpl : ILoanRepository
    {
        private SqlConnection _connection;

        public LoanRepositoryImpl()
        {
            _connection = DBConnUtil.GetDBConnection();
        }

        public void ApplyLoan(Loan loan)
        {

            Console.Write("Do you want to apply for the loan? (Yes/No): ");
            string confirmation = Console.ReadLine();

            if (!confirmation.Equals("Yes", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Loan application cancelled.");
                return;
            }

            try
            {
                _connection.Open();

                string query = @"INSERT INTO Loan 
                         (LoanId, CustomerId, PrincipalAmount, InterestRate, LoanTerm, LoanType, LoanStatus,
                          PropertyAddress, PropertyValue, CarModel, CarValue)
                         VALUES 
                         (@LoanId, @CustomerId, @PrincipalAmount, @InterestRate, @LoanTerm, @LoanType, @LoanStatus,
                          @PropertyAddress, @PropertyValue, @CarModel, @CarValue)";

                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@LoanId", loan.LoanId);
                    cmd.Parameters.AddWithValue("@CustomerId", loan.Customer.CustomerId);
                    cmd.Parameters.AddWithValue("@PrincipalAmount", loan.PrincipalAmount);
                    cmd.Parameters.AddWithValue("@InterestRate", loan.InterestRate);
                    cmd.Parameters.AddWithValue("@LoanTerm", loan.LoanTerm);
                    cmd.Parameters.AddWithValue("@LoanType", loan.LoanType);
                    cmd.Parameters.AddWithValue("@LoanStatus", "Pending");

                    if (loan is HomeLoan homeLoan)
                    {
                        cmd.Parameters.AddWithValue("@PropertyAddress", homeLoan.PropertyAddress ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PropertyValue", homeLoan.PropertyValue);
                        cmd.Parameters.AddWithValue("@CarModel", DBNull.Value);
                        cmd.Parameters.AddWithValue("@CarValue", DBNull.Value);
                    }
                    else if (loan is CarLoan carLoan)
                    {
                        cmd.Parameters.AddWithValue("@PropertyAddress", DBNull.Value);
                        cmd.Parameters.AddWithValue("@PropertyValue", DBNull.Value);
                        cmd.Parameters.AddWithValue("@CarModel", carLoan.CarModel ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CarValue", carLoan.CarValue);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PropertyAddress", DBNull.Value);
                        cmd.Parameters.AddWithValue("@PropertyValue", DBNull.Value);
                        cmd.Parameters.AddWithValue("@CarModel", DBNull.Value);
                        cmd.Parameters.AddWithValue("@CarValue", DBNull.Value);
                    }

                    int result = cmd.ExecuteNonQuery();
                    Console.WriteLine(result > 0 ? "Loan applied successfully!" : "Loan application failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }

        public double CalculateInterest(int loanId)
        {

            try
            {
                _connection.Open();
                string query = "SELECT PrincipalAmount, InterestRate, LoanTerm FROM Loan WHERE LoanId = @LoanId";

                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@LoanId", loanId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            double principal = Convert.ToDouble(reader["PrincipalAmount"]);
                            double rate = Convert.ToDouble(reader["InterestRate"]);
                            int months = Convert.ToInt32(reader["LoanTerm"]);

                            return CalculateInterest(principal, rate, months); // overload
                        }
                        else
                        {
                            throw new InvalidLoanException("Loan not found for ID: " + loanId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return 0;
            }
            finally
            {
                _connection.Close();
            }
        }

        public double CalculateInterest(double principal, double rate, int months)
        {
            return (principal * rate * months) / 12;
        }

        public string LoanStatus(int loanId)
        {
            try
            {
                _connection.Open();

                string query = @"SELECT c.CreditScore 
                         FROM Loan l 
                         JOIN Customer c ON l.CustomerId = c.CustomerId 
                         WHERE l.LoanId = @LoanId";

                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@LoanId", loanId);
                    object result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        throw new InvalidLoanException("Loan not found.");
                    }

                    int creditScore = Convert.ToInt32(result);
                    string status = creditScore > 650 ? "Approved" : "Rejected";

                    // Update status in DB
                    string updateQuery = "UPDATE Loan SET LoanStatus = @Status WHERE LoanId = @LoanId";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, _connection))
                    {
                        updateCmd.Parameters.AddWithValue("@Status", status);
                        updateCmd.Parameters.AddWithValue("@LoanId", loanId);
                        updateCmd.ExecuteNonQuery();
                    }

                    return status;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return "Error";
            }
            finally
            {
                _connection.Close();
            }
        }

        public double CalculateEMI(int loanId)
        {
            try
            {
                _connection.Open();

                string query = "SELECT PrincipalAmount, InterestRate, LoanTerm FROM Loan WHERE LoanId = @LoanId";
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@LoanId", loanId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            double principal = Convert.ToDouble(reader["PrincipalAmount"]);
                            double rate = Convert.ToDouble(reader["InterestRate"]);
                            int months = Convert.ToInt32(reader["LoanTerm"]);

                            return CalculateEMI(principal, rate, months); // call overloaded version
                        }
                        else
                        {
                            throw new InvalidLoanException("Loan not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return 0;
            }
            finally
            {
                _connection.Close();
            }
        }

        public double CalculateEMI(double principal, double rate, int months)
        {
            double monthlyRate = rate / 12 / 100;
            double emi = (principal * monthlyRate * Math.Pow(1 + monthlyRate, months)) /
                         (Math.Pow(1 + monthlyRate, months) - 1);
            return emi;
        }

        public void LoanRepayment(int loanId, double amount)
        {
            try
            {
                double emi = CalculateEMI(loanId);

                if (emi == 0)
                {
                    Console.WriteLine("Cannot calculate EMI. Repayment failed.");
                    return;
                }

                int numEMIs = (int)(amount / emi);

                if (numEMIs < 1)
                {
                    Console.WriteLine("Amount is less than one EMI. Payment rejected.");
                    return;
                }

                Console.WriteLine($"Repayment successful. You have paid {numEMIs} EMIs.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public List<Loan> GetAllLoans()
        {
            List<Loan> loans = new List<Loan>();

            try
            {
                _connection.Open();

                string query = "SELECT * FROM Loan";
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Loan loan = CreateLoanFromReader(reader);
                            loans.Add(loan);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }

            return loans;
        }

        public Loan GetLoanById(int loanId)
        {
            try
            {
                _connection.Open();

                string query = "SELECT * FROM Loan WHERE LoanId = @LoanId";
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@LoanId", loanId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateLoanFromReader(reader);
                        }
                        else
                        {
                            throw new InvalidLoanException("Loan not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
            finally
            {
                _connection.Close();
            }
        }

        private Loan CreateLoanFromReader(SqlDataReader reader)
        {
            int loanId = Convert.ToInt32(reader["LoanId"]);
            int customerId = Convert.ToInt32(reader["CustomerId"]);
            double principal = Convert.ToDouble(reader["PrincipalAmount"]);
            double rate = Convert.ToDouble(reader["InterestRate"]);
            int term = Convert.ToInt32(reader["LoanTerm"]);
            string type = reader["LoanType"].ToString();
            string status = reader["LoanStatus"].ToString();

            Customer dummyCustomer = new Customer { CustomerId = customerId };

            if (type == "HomeLoan")
            {
                string addr = reader["PropertyAddress"]?.ToString();
                int val = Convert.ToInt32(reader["PropertyValue"]);
                return new HomeLoan(loanId, dummyCustomer, principal, rate, term, status, addr, val);
            }
            else if (type == "CarLoan")
            {
                string model = reader["CarModel"]?.ToString();
                int val = Convert.ToInt32(reader["CarValue"]);
                return new CarLoan(loanId, dummyCustomer, principal, rate, term, status, model, val);
            }
            else
            {
                return new Loan(loanId, dummyCustomer, principal, rate, term, type, status);
            }
        }

    }
}

