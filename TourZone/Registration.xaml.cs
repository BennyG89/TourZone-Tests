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
using System.Windows.Shapes;

namespace TourZone
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        TourZone_DevEntities db = new TourZone_DevEntities(@"metadata=res://*/TourZoneModel.csdl|res://*/TourZoneModel.ssdl|res://*/TourZoneModel.msl;provider=System.Data.SqlClient;provider connection string='data source = (LocalDb)\TourZone_Dev; initial catalog = TourZone_Dev; persist security info=True;user id = sa; password=Password!23;MultipleActiveResultSets=True;App=EntityFramework'");

        RegistrationProcess registrationProcess = new RegistrationProcess();

        public Registration()
        {
            InitializeComponent();
        }

        /// <summary>
        /// build useraccount and register for system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (registrationProcess.ValidateUsernamePassword(txtUsername.Text, txtPassword.Password))
            {
                UserAccount userAccount = new UserAccount();
                userAccount.Forename = txtForename.Text;
                userAccount.Surname = txtSurname.Text;
                userAccount.DateOfBirth = dtpDateOfBirth.SelectedDate;
                userAccount.Username = txtUsername.Text;

                if (rdoCompany.IsChecked == true)
                {
                    userAccount.AccountTypeId = db.AccountTypes.Where(x => x.AccountTypeName == "Company").FirstOrDefault().AccountTypeId;
                }
                else if (rdoJobSeeker.IsChecked == true)
                {
                    userAccount.AccountTypeId = db.AccountTypes.Where(x => x.AccountTypeName == "JobSeeker").FirstOrDefault().AccountTypeId;
                }

                if (txtPassword.Password == txtPasswordConfirm.Password)
                {
                    userAccount.Password = txtPassword.Password;
                }
                else
                {
                    MessageBox.Show("Passwords do not match.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                userAccount.Email = txtEmail.Text;

                int saveSuccess = SaveUserAccount(userAccount);

                if (saveSuccess == 1)
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Problem saving record.", "Save to database", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Username or password are not valid. Check length.", "Username and password error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// save useraccount to database
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public int SaveUserAccount(UserAccount userAccount)
        {
            db.Entry(userAccount).State = System.Data.Entity.EntityState.Added;

            int saveSuccess = db.SaveChanges();
            return saveSuccess;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }
               
    }
}
