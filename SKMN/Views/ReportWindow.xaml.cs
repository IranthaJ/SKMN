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


            foreach(DataRow row in recordsFromDB.Rows)
            {
                DateTime? recordDate = row[2] is DBNull ? null : (DateTime?)row[2];
                DateTime companyStartTime = recordDate.Value.Add(new TimeSpan(8, 30, 0));
                DateTime companyEndTime = recordDate.Value.Add(new TimeSpan(17, 30, 0));

                DateTime? signin = row[3] is DBNull ? null : (DateTime?)row[3];
                DateTime? signout = row[4] is DBNull ? null : (DateTime?)row[4];
                reporter.Items.Add(new ReportRecord(Convert.ToInt32(row[1]), recordDate, signin, signout, companyStartTime, companyEndTime));
            }
            //reporter.Items.Add(new ReportRecord(85685,new DateTime(2016,6,14),new DateTime(2016,6,14,8,2,26),new DateTime(2016,6,14,17,36,45),new DateTime(2016,6,14,8,0,0),new DateTime(2016,6,14,17,0,0)));
            //reporter.Items.Add(new ReportRecord(85685, new DateTime(2016, 6, 14), new DateTime(2016, 6, 14, 7, 52, 26), new DateTime(2016, 6, 14, 17, 10, 45), new DateTime(2016, 6, 14, 8, 0, 0), new DateTime(2016, 6, 14, 17, 0, 0)));
        }
    }
}
