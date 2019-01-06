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
    /// Interaction logic for JobSeekerIndex.xaml
    /// </summary>
    public partial class JobSeekerIndex : Page
    {

        TourZone_DevEntities db = new TourZone_DevEntities(@"metadata=res://*/TourZoneModel.csdl|res://*/TourZoneModel.ssdl|res://*/TourZoneModel.msl;provider=System.Data.SqlClient;provider connection string='data source = (LocalDb)\TourZone_Dev; initial catalog = TourZone_Dev; persist security info=True;user id = sa; password=Password!23;MultipleActiveResultSets=True;App=EntityFramework'");

        List<JobSeeker> jobSeekers = new List<JobSeeker>();
        public Dashboard dashboard = new Dashboard();
        public UserAccount user = new UserAccount();

        public JobSeekerIndex()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData(true);
        }
        /// <summary>
        /// load jobseekers from database, if initial load do not search the criteria
        /// </summary>
        /// <param name="initialLoad"></param>
        public void LoadData(bool initialLoad)
        {
            jobSeekers = new List<JobSeeker>();
            lstJobSeekers.ItemsSource = jobSeekers;

            if (initialLoad)
            {
                foreach (var jobSeeker in db.JobSeekers.Where(x => (x.IsDeleted == false || x.IsDeleted == null)).ToList())
                {
                    jobSeekers.Add(jobSeeker);
                }
            }
            else
            {
                if(txtJobTitleSearch.Text != "")
                {

                    foreach (var jobSeeker in db.JobSeekers.Where(x => x.JobTitle.ToLower().Contains(txtJobTitleSearch.Text.ToLower()) &&
                                                                                 x.AvailableNow == ckAvailableNowFilter.IsChecked &&
                                                                                 x.SeekingWork == ckSeekingWorkFilter.IsChecked &&
                                                                                 (x.IsDeleted == false || x.IsDeleted == null)).ToList())
                    {
                        jobSeekers.Add(jobSeeker);
                    }
                }
                else
                {

                    foreach (var jobSeeker in db.JobSeekers.Where(x => x.AvailableNow == ckAvailableNowFilter.IsChecked &&
                                                                                 x.SeekingWork == ckSeekingWorkFilter.IsChecked &&
                                                                                 (x.IsDeleted == false || x.IsDeleted == null)).ToList())
                    {
                        jobSeekers.Add(jobSeeker);
                    }
                }
            }
            
        }
        /// <summary>
        /// when jobseeker is selected view their page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            JobSeeker js = (JobSeeker)lstJobSeekers.SelectedItems[0];
            if (js != null)
            {
                JobSeekerEdit jobSeekerEdit = new JobSeekerEdit();
                jobSeekerEdit.user = user;
                jobSeekerEdit.jobSeekerId = js.JobSeekerId;
                dashboard.frmMain.Navigate(jobSeekerEdit);
            }
        }
        /// <summary>
        /// reload the data on checkbox change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckAvailableNowFilter_Changed(object sender, RoutedEventArgs e)
        {
            LoadData(false);
        }
        /// <summary>
        /// reload the data on checkbox change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckSeekingWorkFilter_Changed(object sender, RoutedEventArgs e)
        {
            LoadData(false);
        }
        /// <summary>
        /// reload the data on search button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            LoadData(false);
        }
    }
}
