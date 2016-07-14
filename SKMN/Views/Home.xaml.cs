using MyServices;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SKMN.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        string connString = "";
        DBservices dbservice;
        public Home()
        {
            InitializeComponent();
            dbservice = new DBservices();
            connString = Properties.Settings.Default.AttendanceConnectionString;
            List<DateTime> holidays = dbservice.getAllHolidays(connString);
            AddHolidaysToCalandar(holidays);
        }

        public void addOnButtonClickHandler(RoutedEventHandler handler)
        {
            this.btn_attendance.Click += handler;
            this.btn_report.Click += handler;
            this.btn_settings.Click += handler;
        }

        private void AddHolidaysToCalandar(List<DateTime> holidays)
        {
            foreach (DateTime day in holidays)
            {
                myCalandar.BlackoutDates.Add(new CalendarDateRange(day,day));
                //myCalandar.SelectedDates.Add(day);
            }

            //mycalandar.SelectedDates.Add(new DateTime(2016, 6, 15));

        }

    }
}
