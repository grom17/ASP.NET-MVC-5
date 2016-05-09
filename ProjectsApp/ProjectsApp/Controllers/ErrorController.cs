using System;
using System.Web.Mvc;

namespace ProjectsApp.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Index()
        {
            var exception = TempData["Exception"] as Exception;
            if (exception == null)
            {
                exception = Server != null ? Server.GetLastError() : null;
                if (exception != null)
                    View(exception);
                else
                    return RedirectToAction("Index", "Home");
            }
            return View(exception);
        }

        public ActionResult Unauthorized()
        {
            if (Response.StatusCode == 403)
            {
                Response.StatusCode = 200;
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult NotFound()
        {
            if (Response.StatusCode == 404)
            {
                Response.StatusCode = 200;
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
    }
}