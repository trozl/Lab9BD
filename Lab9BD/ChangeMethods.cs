using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab9BD
{
    public class ChangeMethods
    {
        private static String ConStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static SqlConnection conn = new SqlConnection(ConStr);
        public static void ChangeSympthoms(int id, ref DataGridView table)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("");
        }

        public void Method()
        {
            throw new System.NotImplementedException();
        }
    }
}
