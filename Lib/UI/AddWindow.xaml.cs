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
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        Manager manager;
        //string ourWindow;

        public AddWindow(Manager M,string title, DataRowView row)
        {
            InitializeComponent();
            IDTextBox.IsEnabled = false;
            Title = title;
            //ourWindow = title;
            manager = M;
            //IDTextBox.Text =
            //string itemID = row["ID"].ToString(); // replace Column1 with the name of your column
            if (title == "add")
            {
                IDTextBox.Opacity = 0;
            }
            else
            {
                IDTextBox.Text = row["ID"].ToString();
                NameTxtBox.Text = row["Name"].ToString();
                DescTxtBox.Text = row["Description"].ToString();
                PublisherTxtBox.Text = row["Publisher"].ToString();
                PriceTxtBox.Text = row["Price"].ToString();
                DiscountTxtBox.Text = row["Discount"].ToString();
                GenresTxtBox.Text = row["Genre"].ToString();
                AuthorTxtBox.Text = row["Author"].ToString();
                ItemDatePicker.SelectedDate = DateTime.Parse(row["PublishDate"].ToString());
            }
        }

        private void AddItemBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Title == "add")
                {
                    manager.AddItem(NameTxtBox.Text, DescTxtBox.Text, PublisherTxtBox.Text, ItemDatePicker.SelectedDate.Value, PriceTxtBox.Text, DiscountTxtBox.Text, GenresTxtBox.Text, AuthorTxtBox.Text);
                }
                else if (Title == "update")
                {
                    manager.UpdateItem(IDTextBox.Text, NameTxtBox.Text, DescTxtBox.Text, PublisherTxtBox.Text, ItemDatePicker.SelectedDate.Value, PriceTxtBox.Text, DiscountTxtBox.Text, GenresTxtBox.Text, AuthorTxtBox.Text);
                }
                MessageBox.Show(Title + " succseeded");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            HomePage wind = new HomePage(manager);
            this.Close();
            wind.ShowDialog();
        }

    }
}
