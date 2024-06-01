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
    public class Disasie
    {
        public int Disasie_id;
        public String Disasie_Name;
        public String Disasie_Recomendation;
        public int Med_id;
        public int Symp_id;

        private static String ConStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static SqlConnection conn = new SqlConnection(ConStr);

        public Disasie(int disid, string name, string rec, int med, int Symp)
        {
            Disasie_id = disid;
            Disasie_Name = name;
            Disasie_Recomendation= rec;
            Med_id = med;
            Symp_id = Symp;
        }

        public static DataTable DisplayLowDisasie(ref DataGridView table)
        {
            conn.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("USE[MED] Select * from Disasie", conn);
            adapter.Fill(dt);
            table.DataSource = dt;
            conn.Close();
            return dt;


        }

        public static void InsertDisasie(String name, String rec, int med, int symp)
        {
            using (SqlConnection conn = new SqlConnection(ConStr))
            {
                conn.Open();
                SqlCommand command = new SqlCommand($"USE[MED] INSERT INTO Disasie(Болезнь, Рекомендации, Medicine_id, Sympthoms_id) VALUES ('{name}','{rec}', {med},{symp});", conn); ;
                Logger.AddLog(command.CommandText);
                command.ExecuteNonQuery();
                conn.Close() ;
            }
        }

        public void ChangeDisasie()
        {
            if (this != null)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("USE[MED] update Disasie set Болезнь = '" + this.Disasie_Name + "', Рекомендации = '" + this.Disasie_Recomendation + "', Medicine_id = '"+this.Med_id+"', Sympthoms_id = '"+this.Symp_id+"' WHERE Disasie_id = " + this.Disasie_id, conn);
                Logger.AddLog(cmd.CommandText);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            conn.Close();
        }

        public void DeleteDisasie()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("use[MED] delete Disasie WHERE Disasie_id = " + this.Disasie_id, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            Logger.AddLog(cmd.CommandText);
        }
    }
}

