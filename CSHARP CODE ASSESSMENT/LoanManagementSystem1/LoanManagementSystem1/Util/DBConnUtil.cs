using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace LoanManagementSystem1.Util
{
    public class DBConnUtil
    {
        public static SqlConnection GetDBConnection()
        {
            string connectionString = DBPropertyUtil.GetConnectionString("LoanDB");
            return new SqlConnection(connectionString);
        }
    }
}
