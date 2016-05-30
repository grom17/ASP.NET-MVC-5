using System.Web.Mvc;
using XRates.Classes;
using XRates.DAL.EF;
using System.Linq;
using System.Collections.Generic;
using System;
using XRates.BLL;

namespace XRates.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReadRates()
        {
            var data = RateService.Instance.GetRates();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}