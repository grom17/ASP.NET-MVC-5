using SimpleStudentsWebsite.Classes.Helpers;
using System.Web.Mvc;

namespace SimpleStudentsWebsite.Classes.Attributes
{
    public class RoleAttribute : ActionFilterAttribute
    {
        public Roles Access;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            if (filterContext.HttpContext.User.Identity == null || !filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult(System.Web.Security.FormsAuthentication.LoginUrl + "?returnUrl=" +
                filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl));
            }

            if (!CookieHelper.Instance.Role.HasFlag(Access))
                filterContext.HttpContext.Response.StatusCode = 403;
        }
    }
}