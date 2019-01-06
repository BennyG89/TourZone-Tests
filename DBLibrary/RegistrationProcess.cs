using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLibrary
{
    public class RegistrationProcess
    {
        // check if username and password are valid
        public bool ValidateUsernamePassword(string username, string password)
        {
            bool valid = true;

            try
            {
                // check the length of the username is not empty and it is less than 20 characters
                if (username.Length == 0 || username.Length > 20 || username.Contains("!"))
                {
                    valid = false;
                }

                // check that the username contains numbers
                foreach (char ch in username)
                {
                    if (ch >= '0' && ch <= '9')
                    {
                        valid = false;
                    }
                }

                // check the length of the password is not empty and it is less than 20 characters
                if (password.Length == 0 || password.Length > 20)
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
