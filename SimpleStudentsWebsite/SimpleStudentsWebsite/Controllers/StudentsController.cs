using SimpleStudentsWebsite.Classes;
using SimpleStudentsWebsite.Classes.Attributes;
using SimpleStudentsWebsite.Classes.Helpers;
using SimpleStudentsWebsite.DAL;
using SimpleStudentsWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SimpleStudentsWebsite.Controllers
{
    public class StudentsController : BaseController
    {
        // GET: Students
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Students/ReadStudents
        [AllowAnonymous]
        public ActionResult ReadStudents()
        {
            try
            {
                var students = DBHelper.Instance.GetStudentsList().AsEnumerable();
                return PartialView("StudentsList", students);
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Students/StudentDetails
        [Role(Access = Roles.Student |
                       Roles.Teacher)]
        public ActionResult StudentDetails(int Id)
        {
            try
            {
                var student = DBHelper.Instance.GetStudentById(Id);
                return PartialView("StudentDetails", student);
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Students/LoadStudentGrades
        [Role(Access = Roles.Student |
                       Roles.Teacher)]
        public ActionResult LoadStudentGrades(int Id)
        {
            try
            {
                var studentGrades = DBHelper.Instance.GetStudentGradesList(Id);
                return Content(GlobalHelper.Json(studentGrades));
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Students/UpdateStudentDetails
        [Role(Access = Roles.Teacher)]
        [HttpPost]
        public ActionResult UpdateStudentDetails(Students student)
        {
            try
            {
                DBHelper.Instance.UpdateStudent(student);
                return Json(new { success = "Профиль студента обновлен" });
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message });
            }
        }

        // POST: Students/UpdateStudentGrades
        [Role(Access = Roles.Teacher)]
        [HttpPost]
        public ActionResult UpdateStudentGrades(List<StudentGradesModel> grades)
        {
            try
            {
                if (grades == null || grades.Where(x=>x.IsTeacher).Any(g => g.Grade == null))
                    throw new Exception("Введите все оценки");
                DBHelper.Instance.UpdateStudentGrades(grades);
                return Json(new { success = "Оценки студента обновлены" });
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message });
            }
        }

        // Get: Students/PrepareCreateStudent
        [Role(Access = Roles.Teacher)]
        public ActionResult PrepareCreateStudent()
        {
            try
            {
                return PartialView("NewStudent", new NewStudent());
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Students/CreateStudent
        [Role(Access = Roles.Teacher)]
        [HttpPost]
        public ActionResult CreateStudent(Students newStudent)
        {
            try
            {
                CheckModelState(ModelState, "Students.CreateStudent");
                newStudent.Password = AESCrypt.EncryptString(newStudent.Password, "SSWSecretKey");
                var Id = DBHelper.Instance.CreateStudent(newStudent);
                return Json(new { success = "Студент успешно добавлен", Id = Id });
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message });
            }
        }

        // GET: Students/DeleteStudent
        [Role(Access = Roles.Teacher)]
        public ActionResult DeleteStudent(int Id)
        {
            try
            {
                DBHelper.Instance.DeleteStudent(Id);
                return Json(new { success = "Студент успешно удален" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}