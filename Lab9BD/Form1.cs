using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;
using Lab9BD.Properties;

namespace Lab9BD
{
    public partial class Form1 : Form
    {
        private List<string> listSqlCommands = new List<string>();
        private List<Sympthoms> sympthomsList =  new List<Sympthoms>();
        private string Connect = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private DataTable table;
        private Sympthoms sympthomsEdit;
        private Medicine medicineEdit;
        private Disasie disasieEdit;
        private Doctor doctorEdit;
        private Patient patientEdit;
        private Inspection inspectionEdit;
        public Form1()
        {
            InitializeComponent();
            table = Sympthoms.DisplaySympthoms(ref dataGridView1);
            Medicine.DisplayMedicine(ref dataGridView2);
            Disasie.DisplayLowDisasie(ref DisasieDataGrid);
            Doctor.DisplayLowDoctor(ref DoctorDataGridView);
            Patient.DisplayLowPatient(ref PatientDataGrid);
            Inspection.DisplayLowInspection(ref InspectionDataGrid);
            ReportService.DisplayDefaultReport(lab10Grid);
        }

        public ReportService ReportService
        {
            get => default;
            set
            {
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string connstr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            System.Console.WriteLine();
            this.textBox1.Text = Connect;  
        }


        private void createDatabase(string sqlQuerry)
        {
            using(SqlConnection connection = new SqlConnection(Connect))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("create database " + sqlQuerry, connection);
                listSqlCommands.Add(command.CommandText);
                command.ExecuteNonQuery();
            }

        }
        private void AddColumn(string name, string column, string type, string bdname)
        {
            using (SqlConnection connection = new SqlConnection(Connect))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("USE ["+ bdname + "] alter table " + name + " add " + column + " " + type, connection);
                listSqlCommands.Add(command.CommandText);
                command.ExecuteNonQuery();
            }
        }

        private void DeleteColumn(string name, string column, string bdname)
        {
            using( SqlConnection connection = new SqlConnection(Connect))
            {
                connection.Open ();
                SqlCommand command = new SqlCommand("USE[" + bdname + "] Alter table " + name + " drop column " + column +";", connection);
                listSqlCommands.Add(command.CommandText);
                command.ExecuteNonQuery();
            }
        }
        private void CreateTables(string bdname, string name)
        {
            using (SqlConnection connection = new SqlConnection(Connect))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("USE[" + bdname + "] create table " + name +"(" + name+ "_id  INT IDENTITY(1,1) PRIMARY KEY);", connection);
                listSqlCommands.Add (command.CommandText);
                command.ExecuteNonQuery();
            }
        }

        private void DeleteTables(string bdname, string name)
        {
            using(SqlConnection connection = new SqlConnection(Connect))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("USE[" + bdname + "] drop table" + name, connection);
                listSqlCommands.Add(command.CommandText);
                command.ExecuteNonQuery();
            }
        }

        private void Add_FOREIGNKEY(string bdname, string NamePrimary, string NameSecondary)
        {
            using(SqlConnection conn = new SqlConnection(Connect))
            {
                conn.Open();
                SqlCommand commandADD = new SqlCommand("USE[" + bdname + "] alter table " + NamePrimary + " add " + NameSecondary+ "_id int;", conn);
                listSqlCommands.Add(commandADD.CommandText);
                commandADD.ExecuteNonQuery();
                SqlCommand comandForeign = new SqlCommand("USE[" + bdname + "] alter table " + NamePrimary + " add foreign key (" + NameSecondary + "_id) references " + NameSecondary+"("+NameSecondary+"_id);", conn);
                listSqlCommands.Add(comandForeign.CommandText);
                comandForeign.ExecuteNonQuery();

            }
        }
        private void button1_Click(object sender, EventArgs e) => createDatabase(textBox1.Text);
        private void button2_Click(object sender, EventArgs e) => AddColumn(NameBox.Text, ColumnBox.Text, comboBox1.Text, textBox1.Text);

        private void button3_Click(object sender, EventArgs e) => DeleteColumn(NameBox.Text,ColumnBox.Text,textBox1.Text);

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void DeleteTable_Click(object sender, EventArgs e) => DeleteTables(textBox1.Text, TableCreateBox.Text);

        private void CreateTable_Click(object sender, EventArgs e) => CreateTables(textBox1.Text, TableCreateBox.Text);

        private void button5_Click(object sender, EventArgs e) => Add_FOREIGNKEY(textBox1.Text, NameBox.Text, ForeignBox.Text);

        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddSympthom_Click(object sender, EventArgs e)
        {
            Sympthoms.InsertSympthoms(SympthomBox.Text);
            //Sympthoms sympt = new Sympthoms(SympthomBox.Text);
            //sympthomsList.Add(sympt);
            int i = 0;
            Sympthoms.DisplaySympthoms(ref dataGridView1);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SympthomBox.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            sympthomsEdit = new Sympthoms(Convert.ToInt16(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()), dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());

        }

        private void button6_Click(object sender, EventArgs e)
        {
            sympthomsEdit.Sympthoms_name = SympthomBox.Text;
            sympthomsEdit.ChangeSympthoms(ref dataGridView1, ref SympthomBox);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            sympthomsEdit.DeleteSympthoms();
            Sympthoms.DisplaySympthoms(ref dataGridView1);
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void AddMedicine_Click(object sender, EventArgs e)
        {
            Medicine.InsertSympthoms(MedecineNameBox.Text, MedicineDoseBox.Text);
            Medicine.DisplayMedicine(ref dataGridView2);
        }

        private void dataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            MedecineNameBox.Text = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
            MedicineDoseBox.Text = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();
            //sympthomsEdit = new Sympthoms(Convert.ToInt16(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()), dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
            medicineEdit = new Medicine(Convert.ToInt16(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString()), dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString(), dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString());
        }

        private void ChangeMedButton_Click(object sender, EventArgs e)
        {
            medicineEdit.Medicine_Name = MedecineNameBox.Text;
            medicineEdit.Medicie_dose = MedicineDoseBox.Text;
            medicineEdit.ChangeMedicine();
            Medicine.DisplayMedicine(ref dataGridView2);
        }

        private void DeleteMedButton_Click(object sender, EventArgs e)
        {
            medicineEdit.DeleteMedicine();
            Medicine.DisplayMedicine(ref dataGridView2);
        }

        private void AddDisasieButton_Click(object sender, EventArgs e)
        {
            Disasie.InsertDisasie(DisasieNameBox.Text,DisasieRecomendationBox.Text,Convert.ToInt16(DisassieMedBox.Text), Convert.ToInt16(DisasieSympthomBox.Text));
            Disasie.DisplayLowDisasie(ref DisasieDataGrid);
        }
        private void DisasieDataGrid_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DisasieNameBox.Text = DisasieDataGrid.Rows[e.RowIndex].Cells[1].Value.ToString();
            DisasieRecomendationBox.Text = DisasieDataGrid.Rows[e.RowIndex].Cells[2].Value.ToString();
            DisassieMedBox.Text = DisasieDataGrid.Rows[e.RowIndex].Cells[3].Value.ToString();
            DisasieSympthomBox.Text = DisasieDataGrid.Rows[e.RowIndex].Cells[4].Value.ToString();
            //sympthomsEdit = new Sympthoms(Convert.ToInt16(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()), dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
            //medicineEdit = new Medicine(Convert.ToInt16(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString()), dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString(), dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString());
            disasieEdit = new Disasie(Convert.ToInt16(DisasieDataGrid.Rows[e.RowIndex].Cells[0].Value.ToString()), DisasieDataGrid.Rows[e.RowIndex].Cells[1].Value.ToString(), DisasieDataGrid.Rows[e.RowIndex].Cells[2].Value.ToString(), Convert.ToInt16(DisasieDataGrid.Rows[e.RowIndex].Cells[3].Value.ToString()), Convert.ToInt16(DisasieDataGrid.Rows[e.RowIndex].Cells[4].Value.ToString()));
        }
        private void ChangeDisasieButton_Click(object sender, EventArgs e)
        {
            disasieEdit.Disasie_Name = DisasieNameBox.Text;
            disasieEdit.Disasie_Recomendation = DisasieRecomendationBox.Text;
            disasieEdit.Med_id = Convert.ToInt16(DisassieMedBox.Text);
            disasieEdit.Symp_id = Convert.ToInt16(DisasieSympthomBox.Text);
            disasieEdit.ChangeDisasie();
            Disasie.DisplayLowDisasie(ref DisasieDataGrid);
        }

        private void DeleteDisasieButton_Click(object sender, EventArgs e)
        {
            disasieEdit.DeleteDisasie();
            Disasie.DisplayLowDisasie(ref DisasieDataGrid);
        }
        private void DoctorDataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DoctorFirstNBox.Text = DoctorDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
            DoctorLNBox.Text = DoctorDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
            DoctorFathBox.Text = DoctorDataGridView.Rows[e.RowIndex].Cells[3].Value.ToString();
            DoctorPlaceBox.Text = DoctorDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString();
            //sympthomsEdit = new Sympthoms(Convert.ToInt16(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()), dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
            //medicineEdit = new Medicine(Convert.ToInt16(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString()), dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString(), dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString());
            doctorEdit = new Doctor(Convert.ToInt16(DoctorDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString()), DoctorDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString(), DoctorDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString(), DoctorDataGridView.Rows[e.RowIndex].Cells[3].Value.ToString(), DoctorDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Doctor.InsertDoctor(DoctorFirstNBox.Text, DoctorLNBox.Text, DoctorFathBox.Text, DoctorPlaceBox.Text);
            Doctor.DisplayLowDoctor(ref DoctorDataGridView);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            doctorEdit.Doctor_First_Name = DoctorFirstNBox.Text;
            doctorEdit.Doctor_Last_Name = DoctorLNBox.Text;
            doctorEdit.Doctor_Father_Name = DoctorFathBox.Text;
            doctorEdit.Place = DoctorPlaceBox.Text;
            doctorEdit.ChangeDoctor();
            Doctor.DisplayLowDoctor(ref DoctorDataGridView);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            doctorEdit.DeleteDoctor();
            Doctor.DisplayLowDoctor(ref DoctorDataGridView);
        }

        private void PatientAddButton_Click(object sender, EventArgs e)
        {
            Patient.InsertPatient(PatientFristNameBox.Text,PatientLastNameBox.Text,PatientFatherNameBox.Text,PatientSexBox.Text,Convert.ToInt64(PatientPassportBox.Text),Convert.ToInt32(PatientPoliceBox.Text),PatientBirthDayBox.Text);
            Patient.DisplayLowPatient(ref PatientDataGrid);
        }
        private void PatientDataGrid_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            PatientFristNameBox.Text = PatientDataGrid.Rows[e.RowIndex].Cells[4].Value.ToString();
            PatientLastNameBox.Text = PatientDataGrid.Rows[e.RowIndex].Cells[3].Value.ToString();
            PatientFatherNameBox.Text = PatientDataGrid.Rows[e.RowIndex].Cells[5].Value.ToString();
            PatientSexBox.Text = PatientDataGrid.Rows[e.RowIndex].Cells[1].Value.ToString();
            PatientBirthDayBox.Text = PatientDataGrid.Rows[e.RowIndex].Cells[7].Value.ToString();
            PatientPassportBox.Text = PatientDataGrid.Rows[e.RowIndex].Cells[2].Value.ToString();
            PatientPoliceBox.Text = PatientDataGrid.Rows[e.RowIndex].Cells[6].Value.ToString();
            patientEdit = new Patient(Convert.ToInt32(PatientDataGrid.Rows[e.RowIndex].Cells[0].Value.ToString()), PatientDataGrid.Rows[e.RowIndex].Cells[4].Value.ToString(), PatientDataGrid.Rows[e.RowIndex].Cells[3].Value.ToString(), PatientDataGrid.Rows[e.RowIndex].Cells[5].Value.ToString(), PatientDataGrid.Rows[e.RowIndex].Cells[1].Value.ToString(), Convert.ToInt32(PatientDataGrid.Rows[e.RowIndex].Cells[2].Value.ToString()), Convert.ToInt32(PatientDataGrid.Rows[e.RowIndex].Cells[6].Value.ToString()), PatientDataGrid.Rows[e.RowIndex].Cells[7].Value.ToString());
            //sympthomsEdit = new Sympthoms(Convert.ToInt16(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()), dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
            //medicineEdit = new Medicine(Convert.ToInt16(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString()), dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString(), dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString());
            //doctorEdit = new Doctor(Convert.ToInt16(DoctorDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString()), DoctorDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString(), DoctorDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString(), DoctorDataGridView.Rows[e.RowIndex].Cells[3].Value.ToString(), DoctorDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString());
        }

        private void PatientChangeButton_Click(object sender, EventArgs e)
        {
            String mes = PatientSexBox.Text;
            MessageBox.Show(mes[0].ToString());
            patientEdit.Patient_First_Name = PatientFristNameBox.Text;
            patientEdit.Patient_Father_Name = PatientFatherNameBox.Text;
            patientEdit.Patient_Last_Name = PatientLastNameBox.Text;
            patientEdit.Sex = PatientSexBox.Text;
            patientEdit.Police = Convert.ToInt32(PatientPoliceBox.Text);
            patientEdit.Passport = Convert.ToInt32(PatientPassportBox.Text);
            patientEdit.BirthdayDate = PatientBirthDayBox.Text;
            patientEdit.ChangePatient();
            Patient.DisplayLowPatient(ref PatientDataGrid);
            
        }

        private void PatientDeleteButton_Click(object sender, EventArgs e)
        {
            patientEdit.DeletePatient();
            Patient.DisplayLowPatient(ref PatientDataGrid);
        }

        private void tabPage12_Click(object sender, EventArgs e)
        {

        }

        private void AddInspection_Click(object sender, EventArgs e)
        {
            Inspection.InsertInspection(InspectionDateBox.Text,Convert.ToInt16(InspectionDoctorBox.Text),Convert.ToInt16(InspectionPatientBox.Text),Convert.ToInt16(InspectionDisasieBox.Text));
            Inspection.DisplayLowInspection(ref InspectionDataGrid);
        }

        private void InspectionDataGrid_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            InspectionDateBox.Text = InspectionDataGrid.Rows[e.RowIndex].Cells[1].Value.ToString();
            InspectionPatientBox.Text = InspectionDataGrid.Rows[e.RowIndex].Cells[3].Value.ToString();
            InspectionDisasieBox.Text = InspectionDataGrid.Rows[e.RowIndex].Cells[4].Value.ToString();
            InspectionDoctorBox.Text = InspectionDataGrid.Rows[e.RowIndex].Cells[2].Value.ToString();
            inspectionEdit = new Inspection(Convert.ToInt32(InspectionDataGrid.Rows[e.RowIndex].Cells[0].Value.ToString()), Convert.ToInt32(InspectionDataGrid.Rows[e.RowIndex].Cells[4].Value.ToString()), Convert.ToInt32(InspectionDataGrid.Rows[e.RowIndex].Cells[2].Value.ToString()),Convert.ToInt32(InspectionDataGrid.Rows[e.RowIndex].Cells[3].Value.ToString()), InspectionDataGrid.Rows[e.RowIndex].Cells[1].Value.ToString());
            //patientEdit = new Patient(Convert.ToInt32(PatientDataGrid.Rows[e.RowIndex].Cells[0].Value.ToString()), PatientDataGrid.Rows[e.RowIndex].Cells[4].Value.ToString(), PatientDataGrid.Rows[e.RowIndex].Cells[3].Value.ToString(), PatientDataGrid.Rows[e.RowIndex].Cells[5].Value.ToString(), PatientDataGrid.Rows[e.RowIndex].Cells[1].Value.ToString(), Convert.ToInt32(PatientDataGrid.Rows[e.RowIndex].Cells[2].Value.ToString()), Convert.ToInt32(PatientDataGrid.Rows[e.RowIndex].Cells[6].Value.ToString()), PatientDataGrid.Rows[e.RowIndex].Cells[7].Value.ToString());
            //sympthomsEdit = new Sympthoms(Convert.ToInt16(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()), dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
            //medicineEdit = new Medicine(Convert.ToInt16(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString()), dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString(), dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString());
            //doctorEdit = new Doctor(Convert.ToInt16(DoctorDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString()), DoctorDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString(), DoctorDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString(), DoctorDataGridView.Rows[e.RowIndex].Cells[3].Value.ToString(), DoctorDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString());
        }

        private void ChangeInspectionButton_Click(object sender, EventArgs e)
        {
            inspectionEdit.Date = InspectionDateBox.Text;
            inspectionEdit.Patient_id = Convert.ToInt32(InspectionPatientBox.Text);
            inspectionEdit.Doctor_id = Convert.ToInt32(InspectionDoctorBox.Text);
            inspectionEdit.Disasie_id = Convert.ToInt32(InspectionDisasieBox.Text);
            inspectionEdit.ChangeInspection();
            Inspection.DisplayLowInspection(ref InspectionDataGrid);
        }

        private void DeleteInspectionButton_Click(object sender, EventArgs e)
        {
            inspectionEdit.DeleteInspection();
            Inspection.DisplayLowInspection(ref InspectionDataGrid);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ReportService.ExportReportToExcel(lab10Grid);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            ReportService.DisplayDefaultReport(lab10Grid);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            ReportService.BestDoctorReport(lab10Grid, DocFamilylab10.Text);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            ReportService.ExportWord(lab10Grid);
        }
    }
}
