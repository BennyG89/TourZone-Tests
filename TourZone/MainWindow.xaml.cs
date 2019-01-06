using DBLibrary;
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

namespace TourZone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TourZone_DevEntities db = new TourZone_DevEntities(@"metadata=res://*/TourZoneModel.csdl|res://*/TourZoneModel.ssdl|res://*/TourZoneModel.msl;provider=System.Data.SqlClient;provider connection string='data source = (LocalDb)\TourZone_Dev; initial catalog = TourZone_Dev; persist security info=True;user id = sa; password=Password!23;MultipleActiveResultSets=True;App=EntityFramework'");

        LoginProcess loginProcess = new LoginProcess();

        public MainWindow()
        {
            InitializeComponent();
        }


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (loginProcess.ValidateUsernamePassword(username, password))
            {
                foreach (var user in db.UserAccounts)
                {
                    /// verify user and login if credentials are correct
                    if (user.Username == username && user.Password == password)
                    {
                        Dashboard dashboard = new Dashboard();
                        dashboard.user = user;
                        dashboard.Owner = this;
                        dashboard.ShowDialog();
                        this.Hide();
                    }
                    else
                    {
                        lblErrorMessage.Content = "Please check username and password.";
                    }
                }
            }
            else
            {
                MessageBox.Show("Username or password are not valid. Check length.", "Username and password error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        /// <summary>
        /// move to registration page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Owner = this;
            registration.ShowDialog();
            this.Close();
        }

       
    }
}
