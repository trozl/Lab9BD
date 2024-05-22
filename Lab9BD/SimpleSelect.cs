using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab9BD
{
     public class SimpleSelect
    {
        private static String ConStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static SqlConnection conn = new SqlConnection(ConStr);
        
        public static DataTable DisplaySympthoms(ref DataGridView  table)
        {
            conn.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter   adapter = new SqlDataAdapter("USE[MED] Select * from Sympthoms", conn);
            adapter.Fill(dt);
            table.DataSource = dt;
            conn.Close();
            return dt;


        }
    }
}
