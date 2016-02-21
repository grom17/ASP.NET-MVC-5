using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleStudentsWebsite.Controllers;
using System.Collections.Generic;
using SimpleStudentsWebsite.DAL;
using SimpleStudentsWebsite.Models;
using SimpleStudentsWebsite.Classes.Helpers;
using System.Linq;

namespace SimpleStudentsWebsite.Tests.Helpers
{
    [TestClass]
    public class DBHelperTest
    {
        [TestMethod]
        public void GetStudentGradesList_NewStudent()
        {
            //Arrange
            int Id = 0; // New student Id
            List<Teachers> teachers = new List<Teachers>() // All teachers
            {
                new Teachers() { TeacherId = 1, Subject = "History" },
                new Teachers() { TeacherId = 2, Subject = "Math" },
                new Teachers() { TeacherId = 3, Subject = "Chemistry" }
            };
            List<Teachers> freeTeachers = new List<Teachers>(); // Teachers without students
            List<Journal> journal = new List<Journal>(); // Journal of grades

            // Act
            List<StudentGradesModel> result = DBHelper.Instance.GetStudentGradesList(teachers, freeTeachers, journal, Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count); // Should return 3 records (from all teachers list)
            result.ForEach(x => Assert.IsFalse(x.IsTeacher)); // IsTeacher flag is always false for new student
        }

        [TestMethod]
        public void GetStudentGradesList_ExistedInJournalStudent()
        {
            //Arrange
            int Id = 1; // student Id
            List<Teachers> teachers = new List<Teachers>() // All teachers list
            {
                new Teachers() { TeacherId = 1, Subject = "History" },
                new Teachers() { TeacherId = 2, Subject = "Math" },
                new Teachers() { TeacherId = 3, Subject = "Chemistry" }
            };
            List<Teachers> freeTeachers = new List<Teachers>() // One teacher without students
            {
                new Teachers() { TeacherId = 3, Subject = "Chemistry" }
            };
            List<Journal> journal = new List<Journal>() // Journal of grades, each teacher has two students
            {
                new Journal() { StudentId = 1, TeacherId = 1, Grade = 4,
                                Teachers = teachers.Where(x=>x.TeacherId.Equals(1)).FirstOrDefault()
                },
                new Journal() { StudentId = 1, TeacherId = 2, Grade = 5,
                                Teachers = teachers.Where(x=>x.TeacherId.Equals(2)).FirstOrDefault()
                }
            };
            bool expectedIsTeacher1 = true;  // Teacher with Id=1 teaches that student
            bool expectedIsTeacher2 = true;  // Teacher with Id=2 teaches that student
            bool expectedIsTeacher3 = false; // Teacher with Id=3 not teaches that student

            // Act
            List<StudentGradesModel> result = DBHelper.Instance.GetStudentGradesList(teachers, freeTeachers, journal, Id);
            bool actualIsTeacher1 = result
                                    .Where(x => x.StudentId.Equals(Id) && x.TeacherId.Equals(1)).ToList()
                                    .Select(x => x.IsTeacher).FirstOrDefault();
            bool actualIsTeacher2 = result
                                    .Where(x => x.StudentId.Equals(Id) && x.TeacherId.Equals(2)).ToList()
                                    .Select(x => x.IsTeacher).FirstOrDefault();
            bool actualIsTeacher3 = result
                                    .Where(x => x.StudentId.Equals(Id) && x.TeacherId.Equals(3)).ToList()
                                    .Select(x => x.IsTeacher).FirstOrDefault();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count); // Should return 3 records (2 teachers from journal and 1 free)
            Assert.AreEqual(expectedIsTeacher1, actualIsTeacher1);
            Assert.AreEqual(expectedIsTeacher2, actualIsTeacher2);
            Assert.AreEqual(expectedIsTeacher3, actualIsTeacher3);
        }

        [TestMethod]
        public void GetStudentGradesList_NotExistedInJournalStudent()
        {
            //Arrange
            int Id = 2; // student Id
            List<Teachers> teachers = new List<Teachers>() // All teachers list
            {
                new Teachers() { TeacherId = 1, Subject = "History" },
                new Teachers() { TeacherId = 2, Subject = "Math" },
                new Teachers() { TeacherId = 3, Subject = "Chemistry" }
            };
            List<Teachers> freeTeachers = new List<Teachers>() // One teacher without students
            {
                new Teachers() { TeacherId = 3, Subject = "Chemistry" }
            };
            List<Journal> journal = new List<Journal>() // Journal of grades, each teacher has two students
            {
                new Journal() { StudentId = 1, TeacherId = 1, Grade = 4,
                                Teachers = teachers.Where(x=>x.TeacherId.Equals(1)).FirstOrDefault()
                },
                new Journal() { StudentId = 1, TeacherId = 2, Grade = 5,
                                Teachers = teachers.Where(x=>x.TeacherId.Equals(2)).FirstOrDefault()
                }
            };

            // Act
            List<StudentGradesModel> result = DBHelper.Instance.GetStudentGradesList(teachers, freeTeachers, journal, Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count); // Should return 3 records (all from total teachers list)
            // No one teacher teaches that student
            result.ForEach(x => Assert.IsFalse(x.IsTeacher));
        }
    }
}
