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
    /// Interaction logic for JobsIndex.xaml
    /// </summary>
    public partial class JobsIndex : Page
    {

        TourZone_DevEntities db = new TourZone_DevEntities(@"metadata=res://*/TourZoneModel.csdl|res://*/TourZoneModel.ssdl|res://*/TourZoneModel.msl;provider=System.Data.SqlClient;provider connection string='data source = (LocalDb)\TourZone_Dev; initial catalog = TourZone_Dev; persist security info=True;user id = sa; password=Password!23;MultipleActiveResultSets=True;App=EntityFramework'");

        List<Job> jobs = new List<Job>();
        Job selectedJob = new Job();
        public UserAccount user = new UserAccount();
        public Dashboard dashboard = new Dashboard();

        public JobsIndex()
        {
            InitializeComponent();
        }  

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
            /// show hide post job button
            if(user.AccountTypeId == 2)
            {
                btnPostJob.Visibility = Visibility.Hidden;
            }
            else
            {
                btnPostJob.Visibility = Visibility.Visible;
            }

            /// load jobs data
            lstJobs.ItemsSource = jobs;            
            foreach (var job in db.Jobs.Where(x => x.IsDeleted == false || x.IsDeleted == null).ToList())
            {
                jobs.Add(job);
            }
        }

        private void ListViewItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            /// show job tab on job selection
            Job j = (Job)lstJobs.SelectedItems[0];
            if (j != null)
            {
                JobEdit jobEdit = new JobEdit();
                jobEdit.selectedJob = j;
                jobEdit.jobId = j.JobId;
                jobEdit.user = user;
                jobEdit.dashboard = dashboard;
                dashboard.frmMain.Navigate(jobEdit);
            }
        }

        private void btnPostJob_Click(object sender, RoutedEventArgs e)
        {
            JobEdit jobEdit = new JobEdit();
            jobEdit.user = user;
            jobEdit.dashboard = dashboard;
            dashboard.frmMain.Navigate(jobEdit);
        }
    }
}
