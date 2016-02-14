﻿using SimpleStudentsWebsite.Classes;
using SimpleStudentsWebsite.Classes.Helpers;
using System;
using System.Web.Mvc;

namespace SimpleStudentsWebsite.Controllers
{
    public class BaseController : Controller
    {
        public ActionResult RedirectToPrevious()
        {
            if (Request.IsAuthenticated)
            {
                string PrevUrl = "";
                if (Session != null)
                    PrevUrl = ((Uri)Session["PrevUrl"]).PathAndQuery;
                if (PrevUrl != null && PrevUrl != "/")
                {
                    string CurrUrl = "";
                    if (Request != null)
                        CurrUrl = Request.Url.PathAndQuery;
                    if (CurrUrl != null && PrevUrl != CurrUrl)
                        return Redirect(PrevUrl);
                }
                Roles role = CookieHelper.Instance.Role;
                if (role.HasFlag(Roles.Dean))
                    return RedirectToAction("Index", "Deans");
                if (role.HasFlag(Roles.Teacher))
                    return RedirectToAction("Index", "Teachers");
                if (role.HasFlag(Roles.Student))
                    return RedirectToAction("Index", "Students");
            }
            return View("~/Views/Home/Index.cshtml");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var httpContext = filterContext.HttpContext;

            if (httpContext.Request.RequestType == "GET"
                && !httpContext.Request.IsAjaxRequest()
                && filterContext.IsChildAction == false)    // do no overwrite if we do child action.
            {
                // stop overwriting previous page if we just reload the current page.
                if (Session != null)
                {
                    if (Session["CurUrl"] != null && ((Uri)Session["CurUrl"]).Equals(httpContext.Request.Url))
                        return;
                    Session["PrevUrl"] = Session["CurUrl"] ?? httpContext.Request.Url;
                    Session["CurUrl"] = httpContext.Request.Url;
                }
            }
        }
    }
}