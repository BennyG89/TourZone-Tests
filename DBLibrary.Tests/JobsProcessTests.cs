using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DBLibrary.Tests
{
    public class JobsProcessTests
    {
        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { "title", 2, "home", 1, "foever", DateTime.Now, true},
            new object[] { "title", 0, "home", 1, "foever", DateTime.Parse("31/12/2019"), false},
            new object[] { "title", 1, "", 0, "foever", DateTime.Today, false},
            new object[] { "title", 3, "home", 1, "", DateTime.Parse("31/1/2019"), false},
            new object[] { "title", 0, "home", 1, "foever", DateTime.Now, false},
            new object[] { "title", 5, "", 1, "foever", DateTime.Parse("31/1/2019"), false},
            new object[] { "title", 8, "", 1, "", DateTime.Now, false},
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void ValidateJob_InputShouldVerify(string title, int categoryId, string location, int companyId, string tourLength, DateTime expiryDate, bool expected)
        {
            //Arrange
            JobsProcess jobsProcess = new JobsProcess();

            //Act
            bool actual = jobsProcess.ValidateJob(title, categoryId, location, companyId, tourLength, expiryDate);

            //Assert
            Assert.Equal(expected, actual);

        }

    }
}
