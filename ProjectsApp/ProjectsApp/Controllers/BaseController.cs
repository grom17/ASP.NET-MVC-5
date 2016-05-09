using System.Web.Mvc;

namespace ProjectsApp.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            var operation = string.Format("{0}.{1}", filterContext.RouteData.Values["controller"], filterContext.RouteData.Values["action"]);
            filterContext.ExceptionHandled = false;
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                string errors = filterContext.Exception.Message;
                errors = filterContext.Exception.InnerException != null ? errors + filterContext.Exception.InnerException.Message : errors;
                filterContext.Result = Json(new { errors = errors }, JsonRequestBehavior.AllowGet);
                filterContext.ExceptionHandled = true;
            }
            else
            {
                filterContext.Result = RedirectToAction("Index", "Error");
                filterContext.ExceptionHandled = true;
                TempData["Exception"] = filterContext.Exception;
            }

            if (!filterContext.ExceptionHandled)
                base.OnException(filterContext);
            else
                filterContext.HttpContext.Response.StatusCode = 200;
        }
    }
}