using SimpleStudentsWebsite.Classes;
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
    }
}