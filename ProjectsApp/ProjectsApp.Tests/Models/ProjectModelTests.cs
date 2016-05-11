using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ProjectsApp.Models;

namespace ProjectsApp.Tests.Models
{
    [TestClass]
    public class ProjectModelTests
    {
        [TestMethod]
        public void ProjectModel_GetDateDisplay()
        {
            DateTime StartDate = new DateTime(2016, 5, 11);
            DateTime EndDate = new DateTime(2016, 5, 12);
            string expectedStartDateDisplay = "5/11/2016";
            string expectedEndDateDisplay = "5/12/2016";

            ProjectModel model = new ProjectModel { StartDate = StartDate, EndDate = EndDate };
            string actualStartDateDisplay = model.StartDateDisplay;
            string actualEndDateDisplay = model.EndDateDisplay;

            Assert.AreEqual(expectedStartDateDisplay, actualStartDateDisplay);
            Assert.AreEqual(expectedEndDateDisplay, actualEndDateDisplay);
        }
    }
}
