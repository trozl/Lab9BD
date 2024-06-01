using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab9BD
{
    public class Inspection
    {
        public int Inspection_id;
        public int Doctor_id;
        public int Patient_id;
        public int Disasie_id;
        public String Date;

        private static String ConStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static SqlConnection conn = new SqlConnection(ConStr);

        public Inspection(int id, int disasie, int doc, int patient, String Date1)
        {
            Inspection_id = id;
            Doctor_id = doc;
            Patient_id = patient;
            Disasie_id = disasie;
            Date = Date1;
        }
        public static DataTable DisplayLowInspection(ref DataGridView table)
        {
            conn.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("USE[MED] Select * from Inspection", conn);
            adapter.Fill(dt);
            table.DataSource = dt;
            conn.Close();
            return dt;


        }

        public static void InsertInspection(string data, int doc, int patient, int dis)
        {
            using (SqlConnection conn = new SqlConnection(ConStr))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("USE[MED] INSERT INTO Inspection(Дата_осмотра, Doctor_id, Patient_id, Disasie_id) VALUES ('" + data + "','" + doc + "','" + patient + "','" + dis + "');", conn);
                Logger.AddLog(command.CommandText);
                command.ExecuteNonQuery();

            }
        }

        public void ChangeInspection()
        {
            if (this != null)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("USE[MED] update Inspection set Дата_осмотра = '" + this.Date + "', Doctor_id = '" + this.Doctor_id + "', Patient_id = '" + this.Patient_id + "', Disasie_id = '" + this.Patient_id + "' WHERE Inspection_id = " + this.Inspection_id, conn);
                Logger.AddLog(cmd.CommandText);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            conn.Close();
        }

        public void DeleteInspection()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("use[MED] delete Inspection WHERE Inspection_id = " + this.Inspection_id, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            Logger.AddLog(cmd.CommandText);
        }
    }
}

