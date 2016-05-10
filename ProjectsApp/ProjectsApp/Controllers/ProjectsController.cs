using ProjectsApp.Classes.Helpers;
using System;
using System.Web.Mvc;

namespace ProjectsApp.Controllers
{
    public class ProjectsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReadProjects()
        {
            try
            {
                var projects = DBHelper.Instance.GetProjectsList();
                return Content(GlobalHelper.Json(projects));
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}