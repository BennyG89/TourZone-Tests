using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLibrary
{
    public class JobsProcess
    {
        // check if username and password are valid
        public bool ValidateJob(string title, int categoryId, string location, int companyId, string tourLength, DateTime expiryDate)
        {
            
            bool valid = true;

            try
            {
                // check the length of the job title is not empty and it is less than 20 characters
                if (title.Length == 0 || title.Length > 30)
                {
                    valid = false;
                }

                // check that the title contains numbers
                foreach (char ch in title)
                {
                    if (ch >= '0' && ch <= '9')
                    {
                        valid = false;
                    }
                }

                // check if categoryId > 0
                if( categoryId <= 0)
                {
                    valid = false;
                }

                // check the length of the location is not empty and it is less than 20 characters
                if (location.Length == 0 || location.Length > 30)
                {
                    valid = false;
                }

                // check if companId > 0
                if (companyId <= 0)
                {
                    valid = false;
                }

                // check the length of the tour length is not empty and it is less than 20 characters
                if (tourLength.Length == 0 || tourLength.Length > 20)
                {
                    valid = false;
                }

                // check to see if the expiry date is valid
                DateTime temp;
                if (DateTime.TryParse(expiryDate.ToString(), out temp) != true)
                {
                    valid = false;
                }
            }
            catch (Exception)
            {
                valid = false;
            }


            return valid;
        }

    }
}
