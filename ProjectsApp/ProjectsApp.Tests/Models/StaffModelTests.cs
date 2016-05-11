using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectsApp.DAL;

namespace ProjectsApp.Tests.Models
{
    [TestClass]
    public class StaffModelTests
    {
        [TestMethod]
        public void StaffModel_GetFullname()
        {
            string FirstName = "Роман";
            string Patronymic = "Александрович";
            string LastName = "Гомозов";
            string expectedFullName = "Р.А.Гомозов";

            Staff model = new Staff { FirstName = FirstName, Patronymic = Patronymic, LastName = LastName };
            string actualFullname = model.Fullname;

            Assert.AreEqual(expectedFullName, actualFullname);
        }
    }
}
