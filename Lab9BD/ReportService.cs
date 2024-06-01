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
    public class ReportService
    {
        private static String ConStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static SqlConnection conn = new SqlConnection(ConStr);
        //private SqlDataAdapter adapter;
        public static void DisplayDefaultReport(DataGridView dataGrid)
        {
            SqlDataAdapter adapter;
            conn.Open();
            DataTable table = new DataTable();
            adapter = new SqlDataAdapter("USE[MED] select Ins.Inspection_id as id, Ins.Дата_осмотра as Дата, doc.Место as Место_приема, pat.Имя as Имя_пациента, pat.Фамилия as Фамилия_пациента, DATEDIFF(YEAR, pat.Дата_рождения, Ins.Дата_осмотра) as Возраст, doc.Фамилия as Фамилия_доктора, doc.Имя as Имя_доктора, symp.Симптомы as Симптомы, dis.Болезнь as Диагноз, medic.Лекарство as Лечение, medic.Доза as Доза_лекарства from dbo.Inspection Ins join Patient pat On Ins.Patient_id = pat.Patient_id join dbo.Doctor doc on ins.Doctor_id = doc.Doctor_id join dbo.Disasie dis on ins.Disasie_id = dis.Disasie_id join dbo.Medicine medic on dis.Medicine_id = medic.Medicine_id join dbo.Sympthoms symp on dis.Sympthoms_id = symp.Sympthoms_id group by Ins.Inspection_id, Ins.Дата_осмотра, pat.Имя, pat.Фамилия, doc.Фамилия, doc.Имя, symp.Симптомы, dis.Болезнь, medic.Лекарство, medic.Доза, doc.Место, pat.Дата_рождения", conn);
            adapter.Fill(table);
            dataGrid.DataSource = table;
            conn.Close();
        }

        public static void BestDoctorReport(DataGridView dataGrid, String DocFamily)
        {
            conn.Open();
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("USE[MED] select Ins.Inspection_id as id, Ins.Дата_осмотра as Дата, doc.Место as Место_приема,\r\npat.Имя as Имя_пациента, pat.Фамилия as Фамилия_пациента, \r\nDATEDIFF(YEAR, pat.Дата_рождения, Ins.Дата_осмотра) as Возраст,\r\ndoc.Фамилия as Фамилия_доктора, doc.Имя as Имя_доктора, \r\nsymp.Симптомы as Симптомы,\r\ndis.Болезнь as Диагноз, medic.Лекарство as Лечение, medic.Доза as Доза_лекарства\r\nfrom dbo.Inspection Ins \r\nJoin Patient pat On Ins.Patient_id = pat.Patient_id \r\njoin dbo.Doctor doc on ins.Doctor_id = doc.Doctor_id \r\njoin dbo.Disasie dis on ins.Disasie_id = dis.Disasie_id\r\njoin dbo.Medicine medic on dis.Medicine_id = medic.Medicine_id\r\njoin dbo.Sympthoms symp on dis.Sympthoms_id = symp.Sympthoms_id Where doc.Фамилия in('" + DocFamily + "') group by Ins.Inspection_id, Ins.Дата_осмотра, pat.Имя, pat.Фамилия, doc.Фамилия, doc.Имя, symp.Симптомы, dis.Болезнь, medic.Лекарство, medic.Доза, doc.Место, pat.Дата_рождения Having DATEDIFF(YEAR, pat.Дата_рождения, Ins.Дата_осмотра) < 20", conn);
            adapter.Fill(table);
            dataGrid.DataSource = table;
            conn.Close();
        }

        public static void ExportReportToExcel(DataGridView dataGrid)
        {
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            app.Visible = true;
            worksheet = workbook.Sheets["Лист1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Exported from Med DB";
            for (int i = 1; i < dataGrid.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = dataGrid.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < dataGrid.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dataGrid.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dataGrid.Rows[i].Cells[j].Value.ToString();
                }
            }
            workbook.SaveAs("E:\\output.xls", Type.Missing);
        }

        public static void ExportWord(DataGridView DGV)
        {
            if (DGV.Rows.Count != 0)
            {
                int RowCount = DGV.Rows.Count;
                int ColumnCount = DGV.Columns.Count;
                Object[,] DataArray = new object[RowCount + 1, ColumnCount + 1];
                //добавим поля и колонки
                int r = 0;
                for (int c = 0; c <= ColumnCount - 1; c++)
                {
                    for (r = 0; r <= RowCount - 1; r++)
                    {
                        DataArray[r, c] = DGV.Rows[r].Cells[c].Value;
                    }
                }
                Microsoft.Office.Interop.Word.Document oDoc = new Microsoft.Office.Interop.Word.Document();
                oDoc.Application.Visible = true;
                //страницы
                oDoc.PageSetup.Orientation =
                Microsoft.Office.Interop.Word.WdOrientation.wdOrientLandscape;
                dynamic oRange = oDoc.Content.Application.Selection.Range;
                string oTemp = "";
                for (r = 0; r <= RowCount - 1; r++)
                {
                    for (int c = 0; c <= ColumnCount - 1; c++)
                    {
                        oTemp = oTemp + DataArray[r, c] + "\t";
                    }
                }

                //формат таблиц
                oRange.Text = oTemp;
                object Separator =
                Microsoft.Office.Interop.Word.WdTableFieldSeparator.wdSeparateByTabs;
                object ApplyBorders = true;
                object AutoFit = true;
                object AutoFitBehavior =
                Microsoft.Office.Interop.Word.WdAutoFitBehavior.wdAutoFitContent;
                oRange.ConvertToTable(ref Separator, ref RowCount, ref ColumnCount,
                Type.Missing, Type.Missing, ref ApplyBorders,
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, ref AutoFit, ref AutoFitBehavior,
                Type.Missing);
                oRange.Select();
                oDoc.Application.Selection.Tables[1].Select();
                oDoc.Application.Selection.Tables[1].Rows.AllowBreakAcrossPages = 0;
                oDoc.Application.Selection.Tables[1].Rows.Alignment = 0;
                oDoc.Application.Selection.Tables[1].Rows[1].Select();
                oDoc.Application.Selection.InsertRowsAbove(1);
                oDoc.Application.Selection.Tables[1].Rows[1].Select();
                //Стили заголовков
                oDoc.Application.Selection.Tables[1].Rows[1].Range.Bold = 1;
                oDoc.Application.Selection.Tables[1].Rows[1].Range.Font.Name = "Tahoma";
                oDoc.Application.Selection.Tables[1].Rows[1].Range.Font.Size = 14;
                for (int c = 0; c <= ColumnCount - 1; c++)
                {
                    oDoc.Application.Selection.Tables[1].Cell(1, c + 1).Range.Text =
                    DGV.Columns[c].HeaderText;
                }
                //Текст заголовка
                foreach (Microsoft.Office.Interop.Word.Section section in
                oDoc.Application.ActiveDocument.Sections)
                {
                    Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    headerRange.Fields.Add(headerRange,
                    Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
                    headerRange.Text = "Отчет";
                    headerRange.Font.Size = 16;
                    headerRange.ParagraphFormat.Alignment =
                    Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                }
            }
        }
    }
}
