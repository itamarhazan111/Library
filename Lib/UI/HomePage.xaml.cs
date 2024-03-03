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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Window
    {
        Manager itamar;
        public HomePage(Manager M)
        {
            itamar = M;
            InitializeComponent();
            if (!M.UserIsAdmin()) {
                SetButtonsAccessibility();
            }
           

            // DataGrid1.ItemsSource = itamar.AddItem("items").AsDataView();
        }

        private void SetButtonsAccessibility()
        {
            AddItemBtb.Visibility = Visibility.Collapsed;
            UpdateItemBtn.Visibility= Visibility.Collapsed;
            DeleteItemBtn.Visibility= Visibility.Collapsed;
            DailyReportBtn.Visibility= Visibility.Collapsed;
            SalesBtn.Visibility= Visibility.Collapsed;

        }
       

        private void AddItemBtb_Click(object sender, RoutedEventArgs e)
        {
            AddWindow wind = new AddWindow(itamar, "add", null);
            this.Close();
            wind.ShowDialog();
        }

        private void UpdateItemBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid1.SelectedItem != null)
            {
                DataRowView row = (DataRowView)DataGrid1.SelectedItem;
                //string itemID = row["ID"].ToString(); // replace Column1 with the name of your column
                // itamar.DeleteFromItems(itemID);
                AddWindow wind = new AddWindow(itamar, "update", row);
                this.Close();
                wind.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please choose table");
            }
        }


        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string category = CategoryCB.Text;
            string table = RadioButtonSelected();
            if (table != null)
            {
                if (category == "Show all")
                {
                    try
                    {
                        DataGrid1.ItemsSource = itamar.ShowAllItems(table).AsDataView();
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
                        DataGrid1.ItemsSource = itamar.ShowItemByParam(SearchItemTxt.Text, category, table).AsDataView();
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please choose table");
            }
            SearchItemTxt.Text = "";
        }

        private string RadioButtonSelected()
        {
            if (RentedRB.IsChecked == true)
                return RentedRB.Content.ToString();
            else if (ItemsRB.IsChecked == true)
                return ItemsRB.Content.ToString();
            return null;
        }

        private void DeleteItemBtn_Click(object sender, RoutedEventArgs e) 
        {   
            if(DataGrid1.SelectedItem != null)
            {
                DataRowView row = (DataRowView)DataGrid1.SelectedItem;
                string itemID = row["ID"].ToString(); 
                itamar.DeleteFromItems(itemID);
                MessageBox.Show(itemID + " deleted");
                DataGrid1.ItemsSource = itamar.ShowAllItems("items").AsDataView();
            }
            else MessageBox.Show("Please select the item you want to delete");
        }

        private void RentItemBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid1.SelectedItem != null)
            {
                DataRowView row = (DataRowView)DataGrid1.SelectedItem;
                if (row != null)
                {
                    string id = row["ID"].ToString();
                    float price = float.Parse(row["Price"].ToString());
                    float discount = float.Parse(row["Discount"].ToString());
                    itamar.RentItem(id);
                    MessageBox.Show($"The {id} rented the final price was {price*(0.01*(100-discount))}");
                    DataGrid1.ItemsSource = itamar.ShowAllItems("items").AsDataView();

                }
                else
                {
                    MessageBox.Show("Please choose valid row");
                }
            }
            else
            {
                MessageBox.Show("Please choose table");
            }
        }

        private void ReturnItemBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid1.SelectedItem != null)
            {
                DataRowView row = (DataRowView)DataGrid1.SelectedItem;
                if (row != null)
                {
                    string itemID = row["ID"].ToString();
                    try
                    {
                        itamar.ReturnItem(itemID);
                        MessageBox.Show(itemID + "  returned");
                    }catch(Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                    }

                    DataGrid1.ItemsSource = itamar.ShowAllItems("rented").AsDataView();
                }
                else
                {
                    MessageBox.Show("Please choose valid row");
                }
            }
            else
            {
                MessageBox.Show("Please choose table");
            }

        }

        private void RentedRB_Checked(object sender, RoutedEventArgs e)
        {
            if (itamar.UserIsAdmin())
            {
                DeleteItemBtn.IsEnabled = false;
                UpdateItemBtn.IsEnabled = false;
            }
            RentItemBtn.IsEnabled = false;
            ReturnItemBtn.IsEnabled = true;
            itamar.UpdateLates();
            DataGrid1.ItemsSource = itamar.ShowAllItems("rented").AsDataView();
        }

        private void ItemsRB_Checked(object sender, RoutedEventArgs e)
        {
            if (itamar.UserIsAdmin())
            {
                DeleteItemBtn.IsEnabled = true;
                UpdateItemBtn.IsEnabled = true;
            }
            ReturnItemBtn.IsEnabled = false;
            RentItemBtn.IsEnabled = true;
            DataGrid1.ItemsSource = itamar.ShowAllItems("items").AsDataView();
        }

        private void DailyReportBtn_Click(object sender, RoutedEventArgs e)
        {
            DailyReport wind = new DailyReport(itamar);
            this.Close();
            wind.ShowDialog();
        }

        private void SalesBtn_Click(object sender, RoutedEventArgs e)
        {
            SalesPage wind = new SalesPage(itamar);
            this.Close();
            wind.ShowDialog();
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow wind = new MainWindow();
            this.Close();
            wind.ShowDialog();
        }
    }
}
