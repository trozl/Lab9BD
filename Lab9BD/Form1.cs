﻿using System;
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

namespace Lab9BD
{
    public partial class Form1 : Form
    {
        private List<string> listSqlCommands = new List<string>();
        private List<Sympthoms> sympthomsList =  new List<Sympthoms>();
        private string Connect = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private DataTable table;
        public Form1()
        {
            InitializeComponent();
            table = SimpleSelect.DisplaySympthoms(ref dataGridView1);
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
            InsertMethods.InsertSympthoms(SympthomBox.Text);
            Sympthoms sympt = new Sympthoms(SympthomBox.Text);
            sympthomsList.Add(sympt);
            int i = 0;
            foreach(Sympthoms item in sympthomsList)
            {
                //dataGridView1.Rows.Add(item.Sympthoms_id, item.Sympthoms_name)
            }
            //dataGridView1.DataSource = sympthomsList;
            SimpleSelect.DisplaySympthoms(ref dataGridView1);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SympthomBox.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

        }
    }
}
