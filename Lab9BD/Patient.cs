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
    internal class Patient
    {
        public int Patient_id;
        public String Patient_First_Name;
        public String Patient_Last_Name;
        public String Patient_Father_Name;
        public String Sex;
        public long Passport;
        public int Police;
        public String BirthdayDate;

        private static String ConStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static SqlConnection conn = new SqlConnection(ConStr);

        public Patient(int id, string name, string fam, string father, string sex, long passport, int police, string Birthday)
        {
            Patient_id = id;
            Patient_First_Name = name;
            Patient_Last_Name = fam;
            Patient_Father_Name = father;
            Sex = sex;
            Passport = passport;
            Police = police;
            BirthdayDate = Birthday;
        }
        public static DataTable DisplayLowPatient(ref DataGridView table)
        {
            conn.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("USE[MED] Select * from Patient", conn);
            adapter.Fill(dt);
            table.DataSource = dt;
            conn.Close();
            return dt;


        }

        public static void InsertPatient(string name, string fam, string father, string sex, long passport, int police, string Birthday)
        {
            using (SqlConnection conn = new SqlConnection(ConStr))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("USE[MED] INSERT INTO Patient(Имя, Фамилия, Отчество, Пол, Пасспортные_данные, Номер_полиса, Дата_рождения) VALUES ('" + name + "','" + fam + "','" + father + "','" + sex + "','"+passport+"','"+police+"','"+Birthday+"');", conn);
                Logger.AddLog(command.CommandText);
                command.ExecuteNonQuery();

            }
        }

        public void ChangePatient()
        {
            if (this != null)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("USE[MED] update Patient set Имя = '" + this.Patient_First_Name + "', Фамилия = '" + this.Patient_Last_Name + "', Отчество = '" + this.Patient_Father_Name + "', Пол = '" + this.Sex + "', Пасспортные_данные = '"+this.Passport+"', Номер_полиса = '"+this.Police+"', Дата_рождения = '"+this.BirthdayDate+"' WHERE Patient_id = " + this.Patient_id, conn);
                Logger.AddLog(cmd.CommandText);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            conn.Close();
        }

        public void DeletePatient()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("use[MED] delete Patient WHERE Patient_id = " + this.Patient_id, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            Logger.AddLog(cmd.CommandText);
        }
    }
}
