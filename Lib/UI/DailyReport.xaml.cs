using Lib;
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

namespace UI
{
    /// <summary>
    /// Interaction logic for DailyReport.xaml
    /// </summary>
    public partial class DailyReport : Window
    {
        Manager manager;
        public DailyReport(Manager M)
        {
            InitializeComponent();
            manager = M;
            ItemsCountTxt.Text = manager.DailyReportCount();
        }

        private void BooksRB_Checked(object sender, RoutedEventArgs e)
        {
            DataGridTable.ItemsSource = manager.ShowAllBooks().AsDataView();
        }

        private void JournalsRB_Checked(object sender, RoutedEventArgs e)
        {
            DataGridTable.ItemsSource = manager.ShowAllJournals().AsDataView();
        }

        private void RentedItemsRB_Checked(object sender, RoutedEventArgs e)
        {
            DataGridTable.ItemsSource = manager.ShowAllRented().AsDataView();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            HomePage wind = new HomePage(manager);
            this.Close();
            wind.ShowDialog();
        }
    }
}
