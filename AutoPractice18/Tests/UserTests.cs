using NUnit.Framework;
using NUnit.Allure.Attributes;
using Atata;

/// <summary>
/// Tests package for Examples of Testing automation 
/// </summary>
namespace AutoPractice18.Tests
{
    /// <summary>
    /// Tests for users page
    /// </summary>
    public class UserTests : UITestFixture
    {
        /// <summary>
        /// Users the create.
        /// </summary>
        [Test]
        [AllureSuite("Users")]
        public void UsersTes()
        {
            var page = Login();

            page.Users.Rows.Should.Contain(row =>
                    row.FirstName == "Jane" &&
                    row.LastName == "Smith");
        }
    }
}
