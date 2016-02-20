using SimpleStudentsWebsite.Classes.Attributes;
using SimpleStudentsWebsite.Classes.Helpers;
using SimpleStudentsWebsite.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SimpleStudentsWebsite.Controllers
{
    [Role(Access = Classes.Roles.Dean)]
    public class ReportsController : BaseController
    {
        // GET: Reports
        public ActionResult Index()
        {
            List<ReportModel> reports = new List<ReportModel>()
            {
                new ReportModel() { Id = 1, ReportName = "Лучшие студенты" },
                new ReportModel() { Id = 2, ReportName = "Преподаватели, обучающие всех студентов" },
                new ReportModel() { Id = 3, ReportName = "Преподаватели с наименьшим количеством студентов" }
            };
            return View(reports);
        }

        // GET: Reports/LoadReport
        public ActionResult LoadReport(int id)
        {
            switch (id)
            {
                case 1:
                    return LoadBestStudentsList();
                case 2:
                    return LoadTeachersOfAllStudentsList();
                case 3:
                    return LoadTeachersOfLowerCountOfStudentsList();
                default: return View("Index");
            }
        }

        // GET: Reports/LoadBestStudentsList
        public ActionResult LoadBestStudentsList()
        {
            var students = DBHelper.Instance.GetBestStudentsList().AsEnumerable();
            return PartialView("StudentsList", students);
        }

        // GET: Reports/LoadTeachersOfAllStudentsList
        public ActionResult LoadTeachersOfAllStudentsList()
        {
            var teachers = DBHelper.Instance.GetTeachersOfAllStudentsList().AsEnumerable();
            return PartialView("TeachersList", teachers);
        }

        // GET: Reports/LoadTeachersOfLowerCountOfStudentsList
        public ActionResult LoadTeachersOfLowerCountOfStudentsList()
        {
            var teachers = DBHelper.Instance.GetTeachersOfLowerCountOfStudentsList().AsEnumerable();
            return PartialView("TeachersList", teachers);
        }
    }
}