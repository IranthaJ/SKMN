using SKMN.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SKMN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Thread.Sleep(500);
            this.home_obj.addOnButtonClickHandler(new RoutedEventHandler(OnUCButtonClickHandler));
        }

        private void OnUCButtonClickHandler(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            if (clickedButton.Name == "btn_attendance")
            {
                hideAllUsercontrols();
                attendance_pannel.Visibility = Visibility.Visible;
            }

            if (clickedButton.Name == "btn_report")
            {
                hideAllUsercontrols();
                report_pannel.Visibility = Visibility.Visible;
            }

            if (clickedButton.Name == "btn_settings")
            {
                hideAllUsercontrols();
                settings_pannel.Visibility = Visibility.Visible;
            }
            
                
        }

        private void hideAllUsercontrols()
        {
            home.Visibility = Visibility.Hidden;
            attendance_pannel.Visibility = Visibility.Hidden;
            report_pannel.Visibility = Visibility.Hidden;
            settings_pannel.Visibility = Visibility.Hidden;
            about_pannel.Visibility = Visibility.Hidden;
        }

        private void MenuItem_Home_Click(object sender, RoutedEventArgs e)
        {
            hideAllUsercontrols();
            home.Visibility = Visibility.Visible;
        }

        private void MenuItem_Attendance_Click(object sender, RoutedEventArgs e)
        {
            hideAllUsercontrols();
            attendance_pannel.Visibility = Visibility.Visible;
        }

        private void MenuItem_Report_Click(object sender, RoutedEventArgs e)
        {
            hideAllUsercontrols();
            report_pannel.Visibility = Visibility.Visible;
        }

        private void MenuItem_Settings_Click(object sender, RoutedEventArgs e)
        {
            hideAllUsercontrols();
            settings_pannel.Visibility = Visibility.Visible;
        }

        private void MenuItem_aboutus_Click(object sender, RoutedEventArgs e)
        {
            hideAllUsercontrols();
            about_pannel.Visibility = Visibility.Visible;
        }

    }
}
