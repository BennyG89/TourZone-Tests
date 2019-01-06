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
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        TourZone_DevEntities db = new TourZone_DevEntities(@"metadata=res://*/TourZoneModel.csdl|res://*/TourZoneModel.ssdl|res://*/TourZoneModel.msl;provider=System.Data.SqlClient;provider connection string='data source = (LocalDb)\TourZone_Dev; initial catalog = TourZone_Dev; persist security info=True;user id = sa; password=Password!23;MultipleActiveResultSets=True;App=EntityFramework'");

        public UserAccount user = new UserAccount();

        public Dashboard()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        private void btnJobs_Click(object sender, RoutedEventArgs e)
        {
            JobsIndex jobsindex = new JobsIndex();
            jobsindex.user = user;
            jobsindex.dashboard = this;
            frmMain.Navigate(jobsindex);
        }

        private void CheckUserAccess(UserAccount user)
        {
            if (user.AccountTypeId == 1)
            {
                btnJobs.Visibility = Visibility.Visible;
                btnPeople.Visibility = Visibility.Visible;
                btnProfile.Visibility = Visibility.Hidden;
                btnCompany.Visibility = Visibility.Visible;
            }

            if (user.AccountTypeId == 2)
            {
                btnJobs.Visibility = Visibility.Visible;
                btnPeople.Visibility = Visibility.Hidden;
                btnProfile.Visibility = Visibility.Visible;
                btnCompany.Visibility = Visibility.Visible;
            }

            if (user.AccountTypeId == 3)
            {
                btnJobs.Visibility = Visibility.Visible;
                btnPeople.Visibility = Visibility.Visible;
                btnProfile.Visibility = Visibility.Visible;
                btnCompany.Visibility = Visibility.Hidden;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CheckUserAccess(this.user);
        }

        private void btnCompany_Click(object sender, RoutedEventArgs e)
        {
            CompanyIndex companyIndex = new CompanyIndex();
            companyIndex.user = user;
            companyIndex.dashboard = this;
            frmMain.Navigate(companyIndex);
        }

        private void btnPeople_Click(object sender, RoutedEventArgs e)
        {
            JobSeekerIndex jobSeekerIndex = new JobSeekerIndex();
            jobSeekerIndex.user = user;
            jobSeekerIndex.dashboard = this;
            frmMain.Navigate(jobSeekerIndex);
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            if(user.AccountTypeId == 2)
            {
                JobSeekerEdit profile = new JobSeekerEdit();
                profile.user = this.user;
                frmMain.Navigate(profile);
            }

            if (user.AccountTypeId == 3)
            {
                CompanyEdit profile = new CompanyEdit();
                profile.user = this.user;
                frmMain.Navigate(profile);
            }
        }
    }
}
