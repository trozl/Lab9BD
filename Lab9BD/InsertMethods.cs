using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Lab9BD
{
    public class InsertMethods
    {
        private  static string ConStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public static void InsertSympthoms(String data) 
        {
            using(SqlConnection conn = new SqlConnection(ConStr)) 
            {
                conn.Open();
                SqlCommand command = new SqlCommand("USE[MED] INSERT INTO Sympthoms (Симптомы) VALUES ('" + data + "');", conn);
                Logger.AddLog(command.CommandText);
                command.ExecuteNonQuery();

            }
        }
    }
}
