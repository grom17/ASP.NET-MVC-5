using SimpleStudentsWebsite.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SimpleStudentsWebsite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_EndRequest()
        {
            var rd = new RouteData();
            IController c = new ErrorController();

            if (Context.Response.StatusCode == 403)
            {
                Response.Clear();
                rd.Values["controller"] = "Error";
                rd.Values["action"] = "Unauthorized";
                c.Execute(new RequestContext(new HttpContextWrapper(Context), rd));
            }
            if (Context.Response.StatusCode == 404)
            {
                Response.Clear();
                rd.Values["controller"] = "Error";
                rd.Values["action"] = "NotFound";
                c.Execute(new RequestContext(new HttpContextWrapper(Context), rd));
            }
            if (Context.Response.StatusCode == 500)
            {
                Response.Clear();
                rd.Values["controller"] = "Error";
                rd.Values["action"] = "Index";
                c.Execute(new RequestContext(new HttpContextWrapper(Context), rd));
            }
        }
    }
}
