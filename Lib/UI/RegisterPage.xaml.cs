using Lib;
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
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Window
    {
        Manager manager;
        public RegisterPage(Manager M)
        {
            InitializeComponent();
            manager = M;
        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            bool? isUs = isAdmin_rb.IsChecked;
            try {
                manager.Register(userName_txt.Text, password_txt.SecurePassword, (bool)isUs);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
           
        }

        private void Cansel_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
