using MyServices;
using SKMN.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Microsoft.Win32;

namespace SKMN.Views
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        private string reportTitle = "";
        private DataServices dataservice;
        string connString = "";
        string errorLog = "";
        DataTable recordsFromDB = new DataTable();
        List<ReportRecord> records = new List<ReportRecord>();
        public ReportWindow()
        {
            InitializeComponent();
        }

        public ReportWindow(int reportType, DateTime startdate, DateTime enddate)
        {
            InitializeComponent();
            connString = Properties.Settings.Default.AttendanceConnectionString;
            dataservice = new DataServices();
            if (reportType == 1)
            {
                this.reportTitle = "Daily report for " + startdate.ToString("dd/MM/yyyy");
                recordsFromDB = dataservice.getRecordsFor(connString, startdate, out errorLog);
            }
            else if (reportType == 2)
            {
                this.reportTitle = "Monthly report for " + startdate.ToString("MMMM - yyyy");
                recordsFromDB = dataservice.getRecordsFor(connString, startdate.Month, startdate.Year,out errorLog);
            }
            else
            {
                this.reportTitle = "Attendance report for " + startdate.ToString("dd/MM/yyyy") + " to " + enddate.ToString("dd/MM/yyyy");
                recordsFromDB = dataservice.getRecordsFor(connString, startdate, enddate, out errorLog);
            }
            lbl_rpt_name.Content = reportTitle;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            ReportRecord rr;
            foreach(DataRow row in recordsFromDB.Rows)
            {
                DateTime? recordDate = row[2] is DBNull ? null : (DateTime?)row[2];
                DateTime companyStartTime = recordDate.Value.Add(new TimeSpan(8, 30, 0));
                DateTime companyEndTime = recordDate.Value.Add(new TimeSpan(17, 30, 0));

                DateTime? signin = row[3] is DBNull ? null : (DateTime?)row[3];
                DateTime? signout = row[4] is DBNull ? null : (DateTime?)row[4];
                rr = new ReportRecord(Convert.ToInt32(row[1]), recordDate, signin, signout, companyStartTime, companyEndTime);
                records.Add(rr);
                reporter.Items.Add(rr);
            }
            //reporter.Items.Add(new ReportRecord(85685,new DateTime(2016,6,14),new DateTime(2016,6,14,8,2,26),new DateTime(2016,6,14,17,36,45),new DateTime(2016,6,14,8,0,0),new DateTime(2016,6,14,17,0,0)));
            //reporter.Items.Add(new ReportRecord(85685, new DateTime(2016, 6, 14), new DateTime(2016, 6, 14, 7, 52, 26), new DateTime(2016, 6, 14, 17, 10, 45), new DateTime(2016, 6, 14, 8, 0, 0), new DateTime(2016, 6, 14, 17, 0, 0)));
        }
        PdfPTable table = new PdfPTable(11);
        private void pdf_report_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = String.Format("{0}.pdf", reportTitle);
            if (saveFileDialog.ShowDialog() == true)
            {
                using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    Document document = new Document(PageSize.A4, 10, 10, 15, 10);
                    document.AddTitle("Attendance Reports");
                    document.AddSubject("Generated report document");
                    document.AddCreator("SKMN Fingerprint Attendance System");
                    document.AddAuthor("Irantha Jayasekara");
                    document.AddHeader("Nothing", "No Header");
                    PdfWriter writer = PdfWriter.GetInstance(document, fs);
                    document.Open();
                    
                    Chunk chunk = new Chunk(reportTitle,FontFactory.GetFont("Tahoma", 14, Font.BOLD));
                    document.Add(chunk);

                    //Phrase phrase = new Phrase("This is from Phrase.");
                    //document.Add(phrase);

                    iTextSharp.text.Paragraph para = new iTextSharp.text.Paragraph(DateTime.Now.ToString("dd/MM/yyyy"), FontFactory.GetFont("Tahoma", 11, Font.BOLD));
                    document.Add(para);


                    //actual width of table in points
                    table.TotalWidth = 580f;
                    //fix the absolute width of the table
                    table.LockedWidth = true;

                    //relative col widths in proportions - 1/3 and 2/3
                    float[] widths = new float[] { 0.8f, 1.2f, 1f, 0.8f, 0.8f, 1f, 0.8f,0.8f, 1f, 1f, 1.9f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = 0;
                    //leave a gap before and after the table
                    table.SpacingBefore = 20f;
                    table.SpacingAfter = 30f;

                    addCell(table, "Emp No", 2);
                    addCell(table, "Date", 2);
                    addCell(table, "In Time", 2);
                    addCell(table, "Late In", 2);
                    addCell(table, "Early In", 2);
                    addCell(table, "Out time", 2);
                    addCell(table, "Early Out", 2);
                    addCell(table, "Late out", 2);
                    addCell(table, "Worked Time", 2);
                    addCell(table, "Over Time", 2);
                    addCell(table, "Status", 2);

                    foreach (ReportRecord row in records)
                    {
                        int type = 1;
                        if (row.Status == "Incorrect record")
                        {
                            type = 3;
                        }
                        addCell(table, row.EmployeeNumber.ToString(), type);
                        addCell(table, row.DisplayRecordDate.ToString(), type);
                        addCell(table, row.DisplayInTime, type);
                        addCell(table, row.LateIn.ToString(), type);
                        addCell(table, row.EarlyIn.ToString(), type);
                        addCell(table, row.DisplayOutTime, type);
                        addCell(table, row.EarlyOut.ToString(), type);
                        addCell(table, row.LateOut.ToString(), type);
                        addCell(table, row.WorkTime, type);
                        addCell(table, row.OverTime, type);
                        addCell(table, row.Status, type);
                    }
                    document.Add(table);

                    document.Close();
                    writer.Close();
                    fs.Close();
                    System.Diagnostics.Process.Start(saveFileDialog.FileName);
                }
            }
        }

        private static void addCell(PdfPTable table, string text, int type)
        {
            PdfPCell cell;
            if (type == 1)
            {
                cell = new PdfPCell(new Phrase(text, FontFactory.GetFont("Tahoma", 10, Font.NORMAL)));
            }
            else if (type == 2)
            {
                cell = new PdfPCell(new Phrase(text, FontFactory.GetFont("Tahoma", 11, Font.BOLD)));
                cell.BackgroundColor = BaseColor.GRAY;  
            }
            else
            {
               // BaseColor myColor = new BaseColor(00052);
                cell = new PdfPCell(new Phrase(text, FontFactory.GetFont("Tahoma", 10, Font.NORMAL)));
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;  
            }
            cell.Rowspan = 1;
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            table.AddCell(cell);
        }


    }
}
