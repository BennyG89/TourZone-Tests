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
    public partial class CompanyIndex:Page
    {

        TourZone_DevEntities db = new TourZone_DevEntities(@"metadata=res://*/TourZoneModel.csdl|res://*/TourZoneModel.ssdl|res://*/TourZoneModel.msl;provider=System.Data.SqlClient;provider connection string='data source = (LocalDb)\TourZone_Dev; initial catalog = TourZone_Dev; persist security info=True;user id = sa; password=Password!23;MultipleActiveResultSets=True;App=EntityFramework'");

        List<Company> companies = new List<Company>();
        public Dashboard dashboard = new Dashboard();
        public UserAccount user = new UserAccount();

        public CompanyIndex()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        /// <summary>
        /// get the list of companies from the database
        /// </summary>
        private void LoadData()
        {
            lstCompanies.ItemsSource = companies;
            foreach (var company in db.Companies.Where(x => x.CompanyName.Contains(txtCompanySearch.Text) &&
                                                        (x.IsDeleted == false || x.IsDeleted == null)).ToList())
            {
                company.PositionsAvailable = db.Jobs.Where(x => x.ExpiryDate < DateTime.Now && x.CompanyId == company.CompanyId && (x.IsDeleted == false || x.IsDeleted == null)).ToList().Count();
                companies.Add(company);
            }
        }
        /// <summary>
        /// when company is selected view their page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            Company c = (Company)lstCompanies.SelectedItems[0];
            if (c != null)
            {
                CompanyEdit companyEdit = new CompanyEdit();
                companyEdit.companyId = c.CompanyId;
                companyEdit.user = user;
                companyEdit.dashboard = dashboard;
                dashboard.frmMain.Navigate(companyEdit);
            }
        }
        /// <summary>
        /// reload data on search button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
