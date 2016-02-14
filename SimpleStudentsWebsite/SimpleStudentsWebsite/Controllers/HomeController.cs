using System.Web.Mvc;

namespace SimpleStudentsWebsite.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToPrevious();
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
    }
}