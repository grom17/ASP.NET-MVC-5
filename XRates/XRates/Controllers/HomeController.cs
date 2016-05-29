using System.Web.Mvc;
using XRates.Classes;
using XRates.DAL.EF;

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
            var rates = DBHelper.Instance.GetRates();
            return Content(GlobalHelper.Json(rates));
        }
    }
}