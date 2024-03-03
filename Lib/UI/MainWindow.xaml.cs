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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Security;
using System.Net;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Manager manager;
        public MainWindow()
        {
            InitializeComponent();
            manager = new Manager();
            //string s = "1234";
            //SecureString pass = new NetworkCredential("", "1234").SecurePassword;
            //SecureString pass=ConvertTo-SecureString
            //manager.Register("Dani", pass, true);
            //HomePage wind = new HomePage(manager);
            //this.Close();
            //wind.ShowDialog();

        }

        private void login_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool logIn = manager.Login(userName_txt.Text, password_txt.SecurePassword);
                if (logIn)
                {
                    HomePage wind = new HomePage(manager);
                    this.Close();
                    wind.ShowDialog();
                }
                else
                {
                    MessageBox.Show("One of the details is not valid");
                }
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
                

        }

        private void register_btn_Click(object sender, RoutedEventArgs e)
        {
            RegisterPage wind = new RegisterPage(manager);
            wind.ShowDialog();
        }
    }
}
