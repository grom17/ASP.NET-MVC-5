using System;
using System.Web.Mvc;

namespace SimpleStudentsWebsite.Controllers
{
    public class ErrorController : BaseController
    {
        // GET: Error
        public ActionResult Index()
        {
            var exception = TempData["Exception"] as Exception;
            if (exception == null)
            {
                exception = Server != null ? Server.GetLastError() : null;
                return exception != null ? View(exception) : RedirectToPrevious();
            }
            return View(exception);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult LogError(string Operation, string Error)
        {
            //Logger.LogError(Operation, Error);
            return Json(true);
        }

        public ActionResult Unauthorized()
        {
            if (Response.StatusCode == 403)
            {
                Response.StatusCode = 200;
                return View();
            }
            return RedirectToPrevious();
        }

        public ActionResult NotFound()
        {
            if (Response.StatusCode == 404)
            {
                Response.StatusCode = 200;
                return View();
            }
            return RedirectToPrevious();
        }
    }
}