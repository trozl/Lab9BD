using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab9BD
{
   public class Sympthoms
    {
        public int Sympthoms_id { get; set; }
        public String Sympthoms_name { get; set; }

        private static String ConStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static SqlConnection conn = new SqlConnection(ConStr);
        public Sympthoms(int sympthoms_id,  string sympthoms_name)
        {
            Sympthoms_id = sympthoms_id;
            Sympthoms_name = sympthoms_name;
        }
        public void ChangeSympthoms(ref DataGridView table, ref TextBox text)
        {
            if (this != null)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("USE[MED] update Sympthoms set Симптомы = '" + this.Sympthoms_name + "' WHERE Sympthoms_id = "+this.Sympthoms_id, conn);
                Logger.AddLog(cmd.CommandText);
                cmd.ExecuteNonQuery();
            }
            conn.Close();
            Sympthoms.DisplaySympthoms(ref table);
        }

        public static DataTable DisplaySympthoms(ref DataGridView table)
        {
            conn.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("USE[MED] Select * from Sympthoms", conn);
            adapter.Fill(dt);
            table.DataSource = dt;
            conn.Close();
            return dt;


        }

        public static void InsertSympthoms(String data)
        {
            using (SqlConnection conn = new SqlConnection(ConStr))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("USE[MED] INSERT INTO Sympthoms (Симптомы) VALUES ('" + data + "');", conn);
                Logger.AddLog(command.CommandText);
                command.ExecuteNonQuery();

            }
        }

        public void DeleteSympthoms()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("use[MED] delete Sympthoms WHERE Sympthoms_id = " + this.Sympthoms_id, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            Logger.AddLog(cmd.CommandText);
        }


    }
}
