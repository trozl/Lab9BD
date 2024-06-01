using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab9BD.Properties
{
    public class Doctor
    {
        public int Doctor_id;
        public String Doctor_First_Name;
        public String Doctor_Last_Name;
        public String Doctor_Father_Name;
        public String Place;

        private static String ConStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static SqlConnection conn = new SqlConnection(ConStr);

        public Doctor(int id, string name, string fam, string father, string place)
        {
            Doctor_id = id;
            Doctor_First_Name = name;
            Doctor_Last_Name = fam;
            Doctor_Father_Name = father;
            Place = place;
        }
        public static DataTable DisplayLowDoctor(ref DataGridView table)
        {
            conn.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("USE[MED] Select * from Doctor", conn);
            adapter.Fill(dt);
            table.DataSource = dt;
            conn.Close();
            return dt;


        }

        public static void InsertDoctor(string name, string fam, string father, string place)
        {
            using (SqlConnection conn = new SqlConnection(ConStr))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("USE[MED] INSERT INTO Doctor(Имя, Фамилия, Отчество, Место) VALUES ('" + name + "','" + fam + "','" + father + "','" + place + "');", conn);
                Logger.AddLog(command.CommandText);
                command.ExecuteNonQuery();

            }
        }

        public void ChangeDoctor()
        {
            if (this != null)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("USE[MED] update Doctor set Имя = '" + this.Doctor_First_Name + "', Фамилия = '" + this.Doctor_Last_Name + "', Отчество = '" + this.Doctor_Father_Name + "', Место = '" + this.Place + "' WHERE Doctor_id = " + this.Doctor_id, conn);
                Logger.AddLog(cmd.CommandText);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            conn.Close();
        }

        public void DeleteDoctor()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("use[MED] delete Doctor WHERE Doctor_id = " + this.Doctor_id, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            Logger.AddLog(cmd.CommandText);
        }
    }
}
