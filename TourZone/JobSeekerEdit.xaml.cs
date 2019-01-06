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
    /// Interaction logic for JobSeeker.xaml
    /// </summary>
    public partial class JobSeekerEdit:Page
    {
        TourZone_DevEntities db = new TourZone_DevEntities(@"metadata=res://*/TourZoneModel.csdl|res://*/TourZoneModel.ssdl|res://*/TourZoneModel.msl;provider=System.Data.SqlClient;provider connection string='data source = (LocalDb)\TourZone_Dev; initial catalog = TourZone_Dev; persist security info=True;user id = sa; password=Password!23;MultipleActiveResultSets=True;App=EntityFramework'");

        public UserAccount user = new UserAccount();
        public int jobSeekerId = 0;
        public JobSeeker jobSeeker = new JobSeeker();
        public JobSeekerEdit()
        {
            InitializeComponent();
        }
        /// <summary>
        /// clear page on button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtName.Text = "";
            txtJobTitle.Text = "";
            txtBio.Text = "";
            txtPhoneNumber.Text = "";
            txtEmail.Text = "";
            ckAvailableNow.IsChecked = false;
            ckSeekingWork.IsChecked = false;
        }
        /// <summary>
        /// build and save jobseeker based on save button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(jobSeeker == null)
            {
                jobSeeker = new JobSeeker();
            }

            jobSeeker.JobTitle = txtJobTitle.Text;
            jobSeeker.ExperienceLevelId = Convert.ToInt32(cboExperienceLevel.SelectedValue);
            jobSeeker.SeekingWork = ckSeekingWork.IsChecked;
            jobSeeker.AvailableNow = ckAvailableNow.IsChecked;
            jobSeeker.Bio = txtBio.Text;
            if(user.AccountTypeId == 2)
            {
                jobSeeker.UserAccountId = user.UserAccountId;
                
            }
            else
            {
                jobSeeker.UserAccountId = db.UserAccounts.Where(x => x.UserAccountId == db.JobSeekers.Where(y => y.JobSeekerId == jobSeekerId).FirstOrDefault().UserAccountId).FirstOrDefault().UserAccountId;
            }

            int saveSuccess = SaveJobSeeker(jobSeeker);

            if(saveSuccess == 1)
            {
                MessageBox.Show("Record saved successfully.", "Save to database", MessageBoxButton.OK, MessageBoxImage.Information);

                UserAccount userToSave = new UserAccount();
                if (user.AccountTypeId == 2)
                {                    
                    userToSave = user;
                }
                else
                {
                    userToSave = db.UserAccounts.Where(x => x.UserAccountId == db.JobSeekers.Where(y => y.JobSeekerId == jobSeekerId).FirstOrDefault().UserAccountId).FirstOrDefault();
                }

                userToSave.CountryCodeId = Convert.ToInt32(cboCountryCode.SelectedValue);
                userToSave.PhoneNumber = txtPhoneNumber.Text;
                //SaveUserAccount(userToSave);
            }
            else
            {
                MessageBox.Show("Problem saving record.", "Save to database", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        /// <summary>
        /// disable jobseeker on disable button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisable_Click(object sender, RoutedEventArgs e)
        {
            JobSeeker jobSeeker = db.JobSeekers.Where(x => x.JobSeekerId == 1).FirstOrDefault();
            jobSeeker.IsActive = false;

            int saveSuccess = SaveJobSeeker(jobSeeker);

            if (saveSuccess == 1)
            {
                MessageBox.Show("Record saved successfully.", "Save to database", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Problem saving record.", "Save to database", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// delete jobseeker on delete button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            JobSeeker jobSeeker = db.JobSeekers.Where(x => x.JobSeekerId == 1).FirstOrDefault();
            jobSeeker.IsDeleted = true;

            int saveSuccess = SaveJobSeeker(jobSeeker);

            if (saveSuccess == 1)
            {
                MessageBox.Show("Record saved successfully.", "Save to database", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Problem saving record.", "Save to database", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// save jobseeker changes to database
        /// </summary>
        /// <param name="jobSeeker"></param>
        /// <returns></returns>
        public int SaveJobSeeker(JobSeeker jobSeeker)
        {
            if(jobSeeker.JobSeekerId > 0)
            {
                db.Entry(jobSeeker).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                db.Entry(jobSeeker).State = System.Data.Entity.EntityState.Added;
            }

            int saveSuccess = db.SaveChanges();
            return saveSuccess;
        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            /// load combobox data
            LoadCountryCodes();
            LoadExperienceLevels();

            /// get data for profile
            if(user.UserAccountId > 0)
            {
                if(user.AccountTypeId == 2)
                {
                    jobSeeker = db.JobSeekers.Where(x => x.UserAccountId == this.user.UserAccountId).FirstOrDefault();

                    btnClear.Visibility = Visibility.Hidden;
                    btnDelete.Visibility = Visibility.Hidden;
                    btnDisable.Visibility = Visibility.Hidden;
                }
            }

            /// if not profile get data on selected jobseekerid
            if (jobSeekerId > 0)
            {
                jobSeeker = db.JobSeekers.Where(x => x.JobSeekerId == jobSeekerId).FirstOrDefault();
            }

            /// load the page with the data else is a new account so allow data input
            if(jobSeeker != null)
            {
                if (jobSeeker.JobSeekerId > 0)
                {
                    LoadData(jobSeeker);
                }
                else
                {
                    cboCountryCode.IsEnabled = true;
                    txtPhoneNumber.IsEnabled = true;
                }
            }
            else
            {
                if(user.UserAccountId > 0)
                {
                    txtName.Text = user.Forename + " " + user.Surname;
                    txtEmail.Text = user.Email;
                }
                cboCountryCode.IsEnabled = true;
                txtPhoneNumber.IsEnabled = true;
            }

            /// company is viewing jobseeker
            if(user.AccountTypeId == 3)
            {
                DisableControls();
            }
            
        }

        private void LoadExperienceLevels()
        {
            cboExperienceLevel.DisplayMemberPath = "ExperienceLevel1";
            cboExperienceLevel.SelectedValuePath = "ExperienceLevelId";
            List<ExperienceLevel> experienceLevels = new List<ExperienceLevel>();
            cboExperienceLevel.ItemsSource = experienceLevels;
            foreach(var level in db.ExperienceLevels)
            {
                experienceLevels.Add(level);
            }
        }
        private void LoadCountryCodes()
        {
            cboCountryCode.DisplayMemberPath = "CountryCode1";
            cboCountryCode.SelectedValuePath = "CountryCodeId";
            List<CountryCode> countryCodes = new List<CountryCode>();
            cboCountryCode.ItemsSource = countryCodes;
            foreach (var code in db.CountryCodes)
            {
                countryCodes.Add(code);
            }
        }
        /// <summary>
        /// load the data to the page
        /// </summary>
        /// <param name="jobSeeker"></param>
        private void LoadData(JobSeeker jobSeeker)
        {
            txtName.Text = jobSeeker.UserAccount.Forename + " " + jobSeeker.UserAccount.Surname;
            cboCountryCode.SelectedIndex = Convert.ToInt32(jobSeeker.UserAccount.CountryCodeId) - 1;
            txtPhoneNumber.Text = jobSeeker.UserAccount.PhoneNumber;
            txtEmail.Text = jobSeeker.UserAccount.Email;

            txtJobTitle.Text = jobSeeker.JobTitle;
            txtBio.Text = jobSeeker.Bio;
            ckAvailableNow.IsChecked = jobSeeker.AvailableNow;
            ckSeekingWork.IsChecked = jobSeeker.SeekingWork;
            cboExperienceLevel.SelectedIndex = Convert.ToInt32(jobSeeker.ExperienceLevelId) - 1;

        }
        /// <summary>
        /// disable controls so the page cannot be edited
        /// </summary>
        private void DisableControls()
        {
            txtBio.IsEnabled = false;
            txtEmail.IsEnabled = false;
            txtJobTitle.IsEnabled = false;
            txtPhoneNumber.IsEnabled = false;
            txtName.IsEnabled = false;
            ckAvailableNow.IsEnabled = false;
            ckSeekingWork.IsEnabled = false;
            cboCountryCode.IsEnabled = false;
            cboExperienceLevel.IsEnabled = false;

            btnClear.Visibility = Visibility.Hidden;
            btnDelete.Visibility = Visibility.Hidden;
            btnDisable.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Hidden;
        }
    }
}
