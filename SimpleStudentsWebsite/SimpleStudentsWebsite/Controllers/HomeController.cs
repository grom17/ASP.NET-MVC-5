using SimpleStudentsWebsite.Classes.Helpers;
using System;
using System.Web.Mvc;

namespace SimpleStudentsWebsite.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "О сайте";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Контакты";

            return View();
        }

        [AllowAnonymous]
        public ActionResult GetDBInfo()
        {
            try
            {
                var studentsCount = DBHelper.Instance.GetStudents().Count;
                var teachersCount = DBHelper.Instance.GetTeachers().Count;
                return Json(new { studentsCount = studentsCount, teachersCount = teachersCount }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}