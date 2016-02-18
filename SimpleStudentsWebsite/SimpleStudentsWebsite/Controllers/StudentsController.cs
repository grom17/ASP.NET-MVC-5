using SimpleStudentsWebsite.Classes;
using SimpleStudentsWebsite.Classes.Attributes;
using SimpleStudentsWebsite.Classes.Helpers;
using SimpleStudentsWebsite.DAL;
using SimpleStudentsWebsite.Models;
using SimpleStudentsWebsite.Models.ViewModels;
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
        [Role(Access = Classes.Roles.Student |
                       Classes.Roles.Teacher)]
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
        [Role(Access = Classes.Roles.Student |
                       Classes.Roles.Teacher)]
        public ActionResult LoadStudentGrades(int Id)
        {
            try
            {
                var studentGrades = new List<StudentGradesModel>();
                if (Id != 0 ) 
                    studentGrades = DBHelper.Instance.GetStudentGradesList(Id);
                return PartialView("StudentGrades", studentGrades);
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Students/GetTeacherById
        [Role(Access = Classes.Roles.Teacher)]
        public ActionResult GetTeacherById(int Id)
        {
            try
            {
                var teacher = DBHelper.Instance.GetTeacherById(Id);
                return Json(new { Fullname = teacher.Fullname, Subject = teacher.Subject }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Students/LoadTeachers
        [Role(Access = Classes.Roles.Teacher)]
        public ActionResult LoadTeachers()
        {
            try
            {
                var teachers = DBHelper.Instance.GetTeachers();
                return PartialView("TeachersList", teachers);
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Students/UpdateStudentDetails
        [Role(Access = Classes.Roles.Teacher)]
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
        [Role(Access = Classes.Roles.Teacher)]
        [HttpPost]
        public ActionResult UpdateStudentGrades(List<StudentGradesModel> grades)
        {
            try
            {
                if (grades == null || grades.Any(g=>g.Grade == null))
                    throw new Exception("Введите все оценки");
                DBHelper.Instance.UpdateStudentGrades(grades);
                return Json(new { success = "Оценки студента обновлены" });
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message });
            }
        }

        // Get: Students/CreateStudent
        [Role(Access = Classes.Roles.Teacher)]
        public ActionResult PrepareCreateStudent()
        {
            try
            {
                return PartialView("NewStudent", new NewStudent());
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message });
            }
        }

        // POST: Students/CreateStudent
        [Role(Access = Classes.Roles.Teacher)]
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
        [Role(Access = Classes.Roles.Teacher)]
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