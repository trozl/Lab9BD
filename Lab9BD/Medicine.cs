using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab9BD
{
    public class Medicine
    {
        public int Medicine_id;
        public String Medicine_Name;
        public String Medicie_dose;

        private static String ConStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static SqlConnection conn = new SqlConnection(ConStr);

        public Medicine(int id, String name, String Dose)
        {
            Medicine_id = id;
            Medicine_Name = name;
            Medicie_dose = Dose;
        }

        public static DataTable DisplayMedicine(ref DataGridView table)
        {
            conn.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("USE[MED] Select * from Medicine", conn);
            adapter.Fill(dt);
            table.DataSource = dt;
            conn.Close();
            return dt;


        }

        public static void InsertSympthoms(String name, String dose)
        {
            using (SqlConnection conn = new SqlConnection(ConStr))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("USE[MED] INSERT INTO Medicine (Лекарство, Доза) VALUES ('" +name + "','"+ dose +"');", conn);
                Logger.AddLog(command.CommandText);
                command.ExecuteNonQuery();

            }
        }

        public void ChangeMedicine()
        {
            if (this != null)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("USE[MED] update Medicine set Лекарство = '" + this.Medicine_Name +"', Доза = '"+this.Medicie_dose+ "' WHERE Medicine_id = " + this.Medicine_id, conn);
                Logger.AddLog(cmd.CommandText);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            conn.Close();
            }

        public void DeleteMedicine()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("use[MED] delete Medicine WHERE Medicine_id = " + this.Medicine_id, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            Logger.AddLog(cmd.CommandText);
        }
    }
}
