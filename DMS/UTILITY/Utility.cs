using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace DMS.UTILITY
{
    public class Utility
    {
        public static SqlConnection GetConnection()
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                SqlConnection conn2 = new SqlConnection(connStr);
                return conn2;
            }
            catch
            {
                throw;
            }
        }
        public static void CloseConnection(SqlConnection conn2)
        {
            try
            {
                if (conn2 != null)
                {
                    conn2.Close();
                    conn2.Dispose();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}