using System;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace TransportManagementSystem.Util
{
    public class DBConnection
    {
        public static SqlConnection GetConnection()
        {
            try
            {
                string connectionString = "Server=RAKS\\SQL2022;Database=TransportDB;Integrated Security=True;Encrypt=False;";


                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database connection error: " + ex.Message);
                return null;
            }
        }
    }
}
