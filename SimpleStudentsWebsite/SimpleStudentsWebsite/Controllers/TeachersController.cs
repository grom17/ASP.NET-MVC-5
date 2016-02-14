using SimpleStudentsWebsite.Classes.Attributes;
using SimpleStudentsWebsite.Classes.Helpers;
using SimpleStudentsWebsite.Models.ViewModels;
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
            List<TeacherModel> teachers = DBHelper.Instance.GetTeachersList();
            return View(teachers);
        }
    }
}