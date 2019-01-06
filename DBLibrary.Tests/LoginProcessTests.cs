using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DBLibrary.Tests
{
    public class LoginProcessTests
    {
        [Theory]
        [InlineData("b", "p", true)]
        [InlineData("s3f", "p", false)]
        [InlineData("b", "", false)]
        [InlineData("bhgf", "hgf", true)]
        [InlineData("", "", false)]
        [InlineData("asdfghjklqwertyuiopzxcvbnm,", "password", false)]
        [InlineData("ben", "123456789012345678900", false)]
        [InlineData("ben!", "password", true)]
        public void ValidateUsernamePassword_StringShouldVerify(string username, string password, bool expected)
        {
            //Arrange
            RegistrationProcess registrationProcess = new RegistrationProcess();

            //Act
            bool actual = registrationProcess.ValidateUsernamePassword(username, password);

            //Assert
            Assert.Equal(expected, actual);

        }

    }
}
