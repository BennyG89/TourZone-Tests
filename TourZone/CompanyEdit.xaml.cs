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
    /// Interaction logic for Company.xaml
    /// </summary>
    public partial class CompanyEdit:Page
    {
        TourZone_DevEntities db = new TourZone_DevEntities(@"metadata=res://*/TourZoneModel.csdl|res://*/TourZoneModel.ssdl|res://*/TourZoneModel.msl;provider=System.Data.SqlClient;provider connection string='data source = (LocalDb)\TourZone_Dev; initial catalog = TourZone_Dev; persist security info=True;user id = sa; password=Password!23;MultipleActiveResultSets=True;App=EntityFramework'");

        public UserAccount user = new UserAccount();
        public Dashboard dashboard = new Dashboard();
        public int companyId = 0;
        public List<Job> companyJobs = new List<Job>();
        public Company company = new Company();

        public CompanyEdit()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Build and save company object on button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (company == null)
            {
                company = new Company();
            }
            
            company.CompanyName = txtName.Text;
            company.Location = txtLocation.Text;
            company.YearFormed = new DateTime(Convert.ToInt32(txtYearFormed.Text), 1, 1);
            company.CompanySizeId = Convert.ToInt32(cboCompanySize.SelectedValue);
            company.CountryCodeId = Convert.ToInt32(cboCountryCode.SelectedValue);
            company.CompanyPhoneNumber = txtPhoneNumber.Text;
            company.CompanyEmail = txtEmail.Text;

            int saveSuccess = SaveCompany(company);

            if (saveSuccess == 1)
            {            
                MessageBox.Show("Record saved successfully.", "Save to database", MessageBoxButton.OK, MessageBoxImage.Information);
                CompanyIndex companyIndex = new CompanyIndex();
                companyIndex.dashboard = dashboard;
                companyIndex.user = user;
                dashboard.frmMain.Navigate(companyIndex);
            }
            else
            {
                MessageBox.Show("Problem saving record.", "Save to database", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        /// <summary>
        /// save company to database
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public int SaveCompany(Company company)
        {
            if(company.CompanyId > 0)
            {
                db.Entry(company).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                db.Entry(company).State = System.Data.Entity.EntityState.Added;
            }

            int saveSuccess = db.SaveChanges();
            return saveSuccess;
        }
        /// <summary>
        /// disable company on button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisable_Click(object sender, RoutedEventArgs e)
        {
            Company company = db.Companies.Where(x => x.CompanyId == 1).FirstOrDefault();
            company.IsActive = false;

            int saveSuccess = SaveCompany(company);

            if (saveSuccess == 1)
            {
                MessageBox.Show("Record saved successfully.", "Save to database", MessageBoxButton.OK, MessageBoxImage.Information);
                CompanyIndex companyIndex = new CompanyIndex();
                companyIndex.dashboard = dashboard;
                dashboard.frmMain.Navigate(companyIndex);
            }
            else
            {
                MessageBox.Show("Problem saving record.", "Save to database", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        /// <summary>
        /// delete company on button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Company company = db.Companies.Where(x => x.CompanyId == 1).FirstOrDefault();
            company.IsDeleted = true;

            int saveSuccess = SaveCompany(company);

            if (saveSuccess == 1)
            {
                CompanyIndex companyIndex = new CompanyIndex();
                companyIndex.dashboard = dashboard;
                dashboard.frmMain.Navigate(companyIndex);
            }
            else
            {
                MessageBox.Show("Problem saving record.", "Save to database", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        /// <summary>
        /// clear all data on button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtName.Text = "";
            txtLocation.Text = "";
            txtYearFormed.Text = "";
            txtPhoneNumber.Text = "";
            txtEmail.Text = "";
            cboCompanySize.SelectedIndex = 1;
            cboCountryCode.SelectedIndex = 1;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            /// set up the comboboxes
            LoadCountryCodes();
            LoadCompanySizes();

            
            if (user.UserAccountId > 0)
            {
                if (user.AccountTypeId == 3)
                {
                    company = db.Companies.Where(x => x.CompanyUserAccounts.Where(y => y.UserAccountId == this.user.UserAccountId).FirstOrDefault().CompanyId == x.CompanyId ).FirstOrDefault();

                    btnClear.Visibility = Visibility.Hidden;
                    btnDelete.Visibility = Visibility.Hidden;
                    btnDisable.Visibility = Visibility.Hidden;

                    companyId = company.CompanyId;
                }
            }
                       

            if (companyId > 0)
            {

                company = db.Companies.Where(x => x.CompanyId == companyId && (x.IsDeleted == false || x.IsDeleted == null)).FirstOrDefault();
                
                /// Load the jobs for the company
                lstJobs.ItemsSource = companyJobs;
                foreach (var job in db.Jobs.Where(x => x.CompanyId == companyId && (x.IsDeleted == false || x.IsDeleted == null)).ToList())
                {
                    companyJobs.Add(job);
                }

            }

            if (company != null)
            {
                /// If company is not new load data, else allow data input
                if (company.CompanyId > 0)
                {
                    LoadData();
                }
                else
                {
                    cboCountryCode.IsEnabled = true;
                    txtPhoneNumber.IsEnabled = true;
                }
            }
            else
            {
                cboCountryCode.IsEnabled = true;
                txtPhoneNumber.IsEnabled = true;
            }

            /// disable controls if this is a jobseeker
            if (user.AccountTypeId == 2)
            {
                DisableControls();
            }
        }
        /// <summary>
        /// LoadData is used to set up the page
        /// </summary>
        private void LoadData()
        {
            txtName.Text = company.CompanyName;
            txtLocation.Text = company.Location;
            txtEmail.Text = company.CompanyEmail;
            txtPhoneNumber.Text = company.CompanyPhoneNumber;
            txtAvailablePostions.Text = company.Jobs.Count.ToString();
            txtYearFormed.Text = company.YearFormed.Value.Year.ToString();
            cboCompanySize.SelectedIndex = Convert.ToInt32(company.CompanySizeId) - 1;
            cboCountryCode.SelectedIndex = Convert.ToInt32(company.CountryCodeId) - 1;
        }
        /// <summary>
        /// Disable controls so page is for viewing only
        /// </summary>
        private void DisableControls()
        {
            txtName.IsEnabled = false;
            txtLocation.IsEnabled = false;
            txtEmail.IsEnabled = false;
            txtPhoneNumber.IsEnabled = false;
            txtAvailablePostions.IsEnabled = false;
            txtYearFormed.IsEnabled = false;
            cboCompanySize.IsEnabled = false;
            cboCountryCode.IsEnabled = false;

            btnClear.Visibility = Visibility.Hidden;
            btnDelete.Visibility = Visibility.Hidden;
            btnDisable.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Hidden;
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

        private void LoadCompanySizes()
        {
            cboCompanySize.DisplayMemberPath = "CompanySize1";
            cboCompanySize.SelectedValuePath = "CompanySizeId";
            List<CompanySize> companySizes = new List<CompanySize>();
            cboCompanySize.ItemsSource = companySizes;
            foreach(var size in db.CompanySizes)
            {
                companySizes.Add(size);
            }
        }
    }
}
