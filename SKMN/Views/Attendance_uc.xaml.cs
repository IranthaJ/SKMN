using Microsoft.Win32;
using MyServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Attendance_uc.xaml
    /// </summary>
    public partial class Attendance_uc : UserControl
    {
        DataServices dataservice;
        string connString = "";
        public Attendance_uc()
        {
            InitializeComponent();
            connString = Properties.Settings.Default.AttendanceConnectionString;
            dataservice = new DataServices();
        }

        private void btn_browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".dat";
            //dlg.Filter = "Data Files (*.dat)|Text Files (*.txt)";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                txt_file.Text = filename;
            }
        }

        private void btn_file_process_Click(object sender, RoutedEventArgs e)
        {
            if (txt_file.Text != "")
            {
                bool result = false;
                string errorLog = "";
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                result = dataservice.processAttendanceFromFile(connString, txt_file.Text,out errorLog);
                Mouse.OverrideCursor = null;

                if(result)
                {
                    MessageBox.Show("data has processed successfully and saved in database", "Process success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(errorLog,"Process error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select the attendance file", "Error - no file selected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
