using System;
using System.Collections.Generic;
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
    /// Interaction logic for Reports_ul.xaml
    /// </summary>
    public partial class Reports_ul : UserControl
    {
        public Reports_ul()
        {
            InitializeComponent();
        }

        private void btn_daily_report_Click(object sender, RoutedEventArgs e)
        {
            if (dt_picker_daily.SelectedDate == null)
            {
                MessageBox.Show("Please select a date                  ", "Select a date", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                ReportWindow rpwnd = new ReportWindow(1,Convert.ToDateTime(dt_picker_daily.SelectedDate), Convert.ToDateTime(dt_picker_daily.SelectedDate));
                rpwnd.ShowDialog();
            }
        }

        private void btn_montly_report_Click(object sender, RoutedEventArgs e)
        {
            if (dt_picker_monthly.SelectedDate == null)
            {
                MessageBox.Show("Please select a month                  ", "Select a month", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                ReportWindow rpwnd = new ReportWindow(2, Convert.ToDateTime(dt_picker_monthly.SelectedDate), Convert.ToDateTime(dt_picker_monthly.SelectedDate));
                rpwnd.ShowDialog();
            }
        }

        private void btn_range_report_Click(object sender, RoutedEventArgs e)
        {
            if (dt_picker_range_from.SelectedDate == null || dt_picker_range_to.SelectedDate == null)
            {
                MessageBox.Show("Please select both 'from' and 'to' dates for your range", "Select dates", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                ReportWindow rpwnd = new ReportWindow(3, Convert.ToDateTime(dt_picker_range_from.SelectedDate), Convert.ToDateTime(dt_picker_range_to.SelectedDate));
                rpwnd.ShowDialog();
            }
        }
    }
}
