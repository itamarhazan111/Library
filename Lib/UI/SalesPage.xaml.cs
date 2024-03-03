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
    public partial class SalesPage : Window
    {
        Manager manager;
        public SalesPage(Manager M)
        {
            InitializeComponent();
            manager = M;
            RefreshDataGrid();
        }

        private void RefreshDataGrid()
        {
            SalesDataGrid.ItemsSource = manager.ShowAllSales().AsDataView();
        }

        private void SaleParamCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SaleParamCB.SelectedIndex == 3)
            {
                SaleParamValTB.IsEnabled = false; ;
                PublishDatePicker.IsEnabled = true;
            }
            else
            {
                SaleParamValTB.IsEnabled = true;
                PublishDatePicker.IsEnabled = false;
            }
        }

        private void AddSaleBtn_Click(object sender, RoutedEventArgs e)
        {
            string saleParam = SaleParamCB.Text;
            if (saleParam == "Publish Date")
            {
                DateTime publishDate;
                try
                {
                    publishDate = (DateTime)PublishDatePicker.SelectedDate;
                    string pubDate = $"{publishDate.Year}-{publishDate.Month}-{publishDate.Day}";
                    manager.AddSale(saleParam, pubDate, ItemDiscountTB.Text);
                    MessageBox.Show("Sale added successfully");
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Please choose date");
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
            else
            {
                try
                {
                    manager.AddSale(saleParam, SaleParamValTB.Text, ItemDiscountTB.Text);
                    MessageBox.Show("Sale added successfully");
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
            RefreshDataGrid();
        }

        private void RemoveSaleBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SalesDataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)SalesDataGrid.SelectedItem;
                string saleID = row["ID"].ToString();
                manager.DeleteSale(saleID);
                MessageBox.Show($"Sale {saleID} deleted");
            }
            else MessageBox.Show("Please select the sale you want to remove");
            RefreshDataGrid();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HomePage wind = new HomePage(manager);
            this.Close();
            wind.ShowDialog();
        }
    }
}
