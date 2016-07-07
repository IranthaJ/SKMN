using Microsoft.Win32;
using MyServices;
using SKMN.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
    /// Interaction logic for Settings_uc.xaml
    /// </summary>
    public partial class Settings_uc : UserControl
    {
        string connString = "";
        DBservices dbservice;
        public Settings_uc()
        {
            InitializeComponent();
            dbservice = new DBservices();
            connString = Properties.Settings.Default.AttendanceConnectionString;
            loadHolidays();
        }

        private void loadHolidays()
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlDataAdapter dataAdap = new SqlDataAdapter();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "Select * from att_holidays Where YEAR ( holiday_date)  = YEAR(GETDATE())";
            dataAdap.SelectCommand = cmd;
            DataSet ds = new DataSet();

            conn.Open();
            dataAdap.Fill(ds);
            conn.Close();
            reloadHolidayList(ds);
        }

        private void reloadHolidayList(DataSet HolidaysSet)
        {
            List<Holiday> holidays = new List<Holiday>();
            foreach (DataRow dr in HolidaysSet.Tables[0].Rows)
            {
                holidays.Add(new Holiday(Convert.ToDateTime(dr["holiday_date"]), Convert.ToString(dr["holiday_reason"]), Convert.ToDateTime(dr["created_date_time"])));
            }

            holidayList.ItemsSource = holidays;
            holidayList.Items.Refresh();
        }

        private void addHoliday(DateTime holiday, string reason)
        {
           
            using (SqlConnection connection = new SqlConnection(connString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;            
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT into att_holidays (holiday_date, holiday_reason, created_date_time) VALUES (@holidate,@reason,@addedtime)";
                    command.Parameters.Add("@holidate", SqlDbType.Date).Value = holiday;
                    command.Parameters.Add("@reason", SqlDbType.NVarChar).Value = reason;
                    command.Parameters.Add("@addedtime", SqlDbType.DateTime).Value = DateTime.Now;

                    try
                    {
                        connection.Open();
                        int recordsAffected = command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        MessageBox.Show(e.Message);
                        // error here
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void btn_backup_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            var sqlConStrBuilder = new SqlConnectionStringBuilder(connString);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = String.Format("{0}-{1}.bak", sqlConStrBuilder.InitialCatalog, DateTime.Now.ToString("yyyy-MM-dd"));
            if (saveFileDialog.ShowDialog() == true)
            {
              result =  dbservice.backupMyDatabase(connString, saveFileDialog.FileName);
            }

            if (result)
            {
                MessageBox.Show("Successfully database backed up", "Back up", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                MessageBox.Show("Error on database backed up", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_holiday_add_Click(object sender, RoutedEventArgs e)
        {
            if (holidate.SelectedDate == null || InputTextBox.Text == "")
            {
                MessageBox.Show("Please select your holiday and reason", "Select date and reason", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                addHoliday(holidate.SelectedDate.Value, InputTextBox.Text.ToString());
                loadHolidays();
                InputTextBox.Text = String.Empty;
                InputBox.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void btn_holiday_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Visible;
        }

        private void btn_holiday_cancel_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Collapsed;
            InputTextBox.Text = String.Empty;
        }

    }
}
