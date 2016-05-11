using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectsApp.DAL;
using System.Collections.Generic;
using ProjectsApp.Classes.Helpers;
using ProjectsApp.Models;
using System;

namespace ProjectsApp.Tests.Helpers
{
    [TestClass]
    public class DBHelperTests
    {
        [TestMethod]
        public void DBHelper_GetEmployeesList()
        {
            List<Staff> employees = new List<Staff> { new Staff { FirstName = "Test1" },
                                                      new Staff { FirstName = "Test2" }
            };

            var result = DBHelper.Instance.GetEmployeesList(employees);

            Assert.IsInstanceOfType(result, typeof(List<EmployeeModel>));
            Assert.AreEqual("Test1", result[0].FirstName);
            Assert.AreEqual("Test2", result[1].FirstName);
        }

        [TestMethod]
        public void DBHelper_GetEmployeeById_EmployeeNotExists_ShouldThrowException()
        {
            List<Staff> employees = new List<Staff> { new Staff { FirstName = "Test1", PersonId = 1 },
                                                      new Staff { FirstName = "Test2", PersonId = 2 }
            };

            try
            {
                var result = DBHelper.Instance.GetEmployeeById(employees, 3);
                Assert.Fail("An exception should have been thrown");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(string.Format(Messages.EmployeeNotExists, 3), ex.Message);
            }          
        }

        [TestMethod]
        public void DBHelper_GetEmployeeById()
        {
            List<Staff> employees = new List<Staff> { new Staff { FirstName = "Test1", PersonId = 1 },
                                                      new Staff { FirstName = "Test2", PersonId = 2 }
            };

            var result = DBHelper.Instance.GetEmployeeById(employees, 2);

            Assert.IsInstanceOfType(result, typeof(EmployeeModel));
            Assert.AreEqual("Test2", result.FirstName);
            Assert.AreEqual(2, result.PersonId);
        }

        [TestMethod]
        public void DBHelper_GetProjectsExecutorsList()
        {
            List<Staff> employees = new List<Staff>
            {
                new Staff { FirstName = "Test1", Patronymic = "Test1", LastName = "Test1", PersonId = 1 },
                new Staff { FirstName = "Test2", Patronymic = "Test2", LastName = "Test2", PersonId = 2 },
                new Staff { FirstName = "Test3", Patronymic = "Test3", LastName = "Test3", PersonId = 3 }
            };
            List<ProjectExecutors> executors = new List<ProjectExecutors>
            {
                new ProjectExecutors { ProjectId = 1, ProjectExecutorId = 1 },
                new ProjectExecutors { ProjectId = 1, ProjectExecutorId = 3 },
                new ProjectExecutors { ProjectId = 2, ProjectExecutorId = 2 },
                new ProjectExecutors { ProjectId = 2, ProjectExecutorId = 1 }
            };

            var result = DBHelper.Instance.GetProjectExecutorsList(employees, executors, 1);

            Assert.IsInstanceOfType(result, typeof(List<EmployeeModel>));
            Assert.AreEqual(1, result[0].PersonId);
            Assert.AreEqual(3, result[1].PersonId);
        }

        [TestMethod]
        public void DBHelper_GetProjectsList()
        {
            List<Staff> employees = new List<Staff>
            {
                new Staff { FirstName = "Test1", Patronymic = "Test1", LastName = "Test1", PersonId = 1 },
                new Staff { FirstName = "Test2", Patronymic = "Test2", LastName = "Test2", PersonId = 2 },
                new Staff { FirstName = "Test3", Patronymic = "Test3", LastName = "Test3", PersonId = 3 }
            };
            List<ProjectExecutors> executors = new List<ProjectExecutors>
            {
                new ProjectExecutors { ProjectId = 1, ProjectExecutorId = 1 },
                new ProjectExecutors { ProjectId = 1, ProjectExecutorId = 3 },
                new ProjectExecutors { ProjectId = 2, ProjectExecutorId = 2 },
                new ProjectExecutors { ProjectId = 2, ProjectExecutorId = 1 }
            };
            List<ProjectInfo> projects = new List<ProjectInfo>
            {
                new ProjectInfo { ProjectId = 1, ProjectManagerId = 1, ClientCompanyName = "Company1",
                                  ExecutiveCompanyName = "Company2", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(2),
                                  Priority = 2, Comment = "Comment1", Staff = employees[0]
                                },
                new ProjectInfo { ProjectId = 2, ProjectManagerId = 2, ClientCompanyName = "Company3",
                                ExecutiveCompanyName = "Company4", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(3),
                                Priority = 3, Comment = "Comment2", Staff = employees[1] 
                                }
            };

            var result = DBHelper.Instance.GetProjectsList(projects, employees, executors);

            Assert.IsInstanceOfType(result, typeof(List<ProjectModel>));
            Assert.AreEqual("T.T.Test1", result[0].ProjectManagerName);
            Assert.AreEqual("T.T.Test2", result[1].ProjectManagerName);
        }
    }
}
