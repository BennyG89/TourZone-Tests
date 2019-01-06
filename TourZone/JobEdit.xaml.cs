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
    public partial class JobEdit : Page
    {

        TourZone_DevEntities db = new TourZone_DevEntities(@"metadata=res://*/TourZoneModel.csdl|res://*/TourZoneModel.ssdl|res://*/TourZoneModel.msl;provider=System.Data.SqlClient;provider connection string='data source = (LocalDb)\TourZone_Dev; initial catalog = TourZone_Dev; persist security info=True;user id = sa; password=Password!23;MultipleActiveResultSets=True;App=EntityFramework'");

        JobsProcess jobsProcess = new JobsProcess();

        public Job selectedJob = new Job();
        public UserAccount user = new UserAccount();
        public Dashboard dashboard = new Dashboard();
        public int jobId = 0;

        public JobEdit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// build and save job
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            DateTime expiryDate = DateTime.Parse(dtpExpiry.SelectedDate.ToString());

            if(jobsProcess.ValidateJob(txtTitle.Text, Convert.ToInt32(cboCategory.SelectedValue), txtLocation.Text, Convert.ToInt32(cboCompany.SelectedValue), txtTourLength.Text, expiryDate))
            {
                Job job = new Job();
                job.JobTitle = txtTitle.Text;
                job.JobCategoryId = Convert.ToInt32(cboCategory.SelectedValue);
                job.Location = txtLocation.Text;
                job.CompanyId = Convert.ToInt32(cboCompany.SelectedValue);
                job.TourLength = txtTourLength.Text;
                job.ExpiryDate = expiryDate;

                int saveSuccess = SaveJob(job);

                if (saveSuccess == 1)
                {
                    MessageBox.Show("Record saved successfully.", "Save to database", MessageBoxButton.OK, MessageBoxImage.Information);
                    JobsIndex jobsIndex = new JobsIndex();
                    jobsIndex.dashboard = dashboard;
                    jobsIndex.user = user;
                    dashboard.frmMain.Navigate(jobsIndex);
                }
                else
                {
                    MessageBox.Show("Problem saving record.", "Save to database", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Problem saving record.", "Save to database", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
        }

        /// <summary>
        /// apply for job
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            JobApplicant jobApplicant = new JobApplicant();

            jobApplicant.JobId = selectedJob.JobId;
            jobApplicant.JobSeekerId = db.JobSeekers.Where(x => x.UserAccountId == user.UserAccountId).FirstOrDefault().JobSeekerId;
            jobApplicant.AppliedForJob = true;

            int saveSuccess = SaveJobApplicant(jobApplicant);

            if (saveSuccess == 1)
            {
                EnableControls();
                JobsIndex jobsIndex = new JobsIndex();
                jobsIndex.dashboard = dashboard;
                jobsIndex.user = user;
                dashboard.frmMain.Navigate(jobsIndex);
            }
            else
            {
                MessageBox.Show("Problem applying for job.", "Job application", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// save job applicant record
        /// </summary>
        /// <param name="jobApplicant"></param>
        /// <returns></returns>
        public int SaveJobApplicant(JobApplicant jobApplicant)
        {
            db.Entry(jobApplicant).State = System.Data.Entity.EntityState.Added;

            int saveSuccess = db.SaveChanges();
            return saveSuccess;
        }

        /// <summary>
        /// delete job on delete button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Job job = db.Jobs.Where(x => x.JobId == 1).FirstOrDefault();
            job.IsDeleted = true;

            int saveSuccess = SaveJob(job);

            if (saveSuccess == 1)
            {
                
                EnableControls();
            }
            else
            {
                MessageBox.Show("Problem saving record.", "Save to database", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        
        public int SaveJob(Job job)
        {
            db.Entry(job).State = System.Data.Entity.EntityState.Added;

            int saveSuccess = db.SaveChanges();
            return saveSuccess;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            /// load comboboxes and data
            LoadJobCategories();
            LoadCompanies();

            if (jobId > 0)
            {
                LoadJobDetails();
                DisableControls();
                btnClear.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
                btnSave.Visibility = Visibility.Hidden;
            }

            if(user.AccountTypeId == 3)
            {                
                cboCompany.SelectedIndex = Convert.ToInt32(db.CompanyUserAccounts.Where(y => y.UserAccountId == user.UserAccountId).FirstOrDefault().CompanyId) - 1;
                cboCompany.IsEnabled = false;
            }

        }        
        
        private void LoadJobCategories()
        {
            cboCategory.DisplayMemberPath = "JobCategory1";
            cboCategory.SelectedValuePath = "JobCategoryId";
            List<JobCategory> jobCategories = new List<JobCategory>();
            cboCategory.ItemsSource = jobCategories;
            foreach (var category in db.JobCategories)
            {
                jobCategories.Add(category);
            }
        }

        private void LoadCompanies()
        {
            cboCompany.DisplayMemberPath = "CompanyName";
            cboCompany.SelectedValuePath = "CompanyId";
            List<Company> companies = new List<Company>();
            cboCompany.ItemsSource = companies;
            foreach (var company in db.Companies)
            {
                companies.Add(company);
            }
        }
        /// <summary>
        /// load selected job details
        /// </summary>
        private void LoadJobDetails()
        {
            txtTitle.Text = selectedJob.JobTitle;
            txtLocation.Text = selectedJob.Location;
            txtTourLength.Text = selectedJob.TourLength;
            dtpExpiry.SelectedDate = selectedJob.ExpiryDate;
            cboCategory.SelectedIndex = Convert.ToInt32(selectedJob.JobCategoryId) - 1;
            cboCompany.SelectedIndex = Convert.ToInt32(selectedJob.CompanyId) - 1;
            if (user.AccountTypeId == 2)
            {
                btnApply.Visibility = Visibility.Visible;
                btnClear.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
                btnSave.Visibility = Visibility.Hidden;
            }
        }

        private void DisableControls()
        {
            txtTitle.IsEnabled = false;
            txtLocation.IsEnabled = false;
            txtTourLength.IsEnabled = false;
            dtpExpiry.IsEnabled = false;
            cboCategory.IsEnabled = false;
            cboCompany.IsEnabled = false;
        }

        private void EnableControls()
        {
            txtTitle.IsEnabled = true;
            txtLocation.IsEnabled = true;
            txtTourLength.IsEnabled = true;
            dtpExpiry.IsEnabled = true;
            cboCategory.IsEnabled = true;
            cboCompany.IsEnabled = true;
            btnClear.Visibility = Visibility.Visible;
            btnDelete.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Visible;
        }
        
        private void ClearControls()
        {
            txtTitle.Text = "";
            txtLocation.Text = "";
            txtTourLength.Text = "";
            cboCategory.SelectedIndex = 1;
            cboCompany.SelectedIndex = 1;
            dtpExpiry.Text = "";
        }
    }
}
