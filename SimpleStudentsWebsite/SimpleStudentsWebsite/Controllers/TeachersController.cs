using SimpleStudentsWebsite.Classes.Attributes;
using SimpleStudentsWebsite.Classes.Helpers;
using SimpleStudentsWebsite.DAL;
using SimpleStudentsWebsite.Models;
using SimpleStudentsWebsite.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SimpleStudentsWebsite.Controllers
{
    public class TeachersController : Controller
    {
        // GET: Teachers
        [AllowAnonymous]
        public ActionResult Index()
        {          
            return View();
        }

        // GET: Teachers/ReadTeachers
        [AllowAnonymous]
        public ActionResult ReadTeachers()
        {
            List<TeacherModel> teachers = DBHelper.Instance.GetTeachersList();
            return PartialView("TeachersList", teachers);
        }

        // Get: Teachers/CreateStudent
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

        // POST: Teachers/CreateStudent
        [Role(Access = Classes.Roles.Teacher)]
        [HttpPost]
        public ActionResult CreateStudent(Students student)
        {
            try
            {
                DBHelper.Instance.CreateStudent(student);
                return Json(new { success = "Студент успешно добавлен" });
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message });
            }
        }
    }
}