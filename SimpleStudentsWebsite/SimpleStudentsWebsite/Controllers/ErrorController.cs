﻿using System.Web.Mvc;

namespace SimpleStudentsWebsite.Controllers
{
    public class ErrorController : BaseController
    {
        // GET: Error
        public ActionResult Index()
        {
            //var exception = Server != null ? Server.GetLastError() : null;
            //if (exception != null)
            //    return View(exception);
            //return RedirectToPrevious();
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult LogError(string Operation, string Error)
        {
            //Logger.LogError(Operation, Error);
            return Json(true);
        }

        //public ActionResult Unauthorized()
        //{
        //    if (Response.StatusCode == 403)
        //        return View();
        //    return RedirectToPrevious();
        //}

        //public ActionResult NotFound()
        //{
        //    if (Response.StatusCode == 404)
        //        return View();
        //    return RedirectToPrevious();
        //}
    }
}