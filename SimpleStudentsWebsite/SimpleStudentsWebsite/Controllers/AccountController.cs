using System.Web.Mvc;
using SimpleStudentsWebsite.Models;
using SimpleStudentsWebsite.Classes.Helpers;
using System.Threading;
using System.Security.Principal;
using System.Web.Security;
using SimpleStudentsWebsite.Classes.Attributes;
using System.Collections.Generic;
using SimpleStudentsWebsite.Models.ViewModels;
using System;

namespace SimpleStudentsWebsite.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                LoginModel loginResult = DBHelper.Instance.Login(model.Login, model.Password);
                var ck = FormsAuthentication.GetAuthCookie(loginResult.Fullname, false);
                HttpContext.Response.Cookies.Add(ck);
                HttpContext.User = Thread.CurrentPrincipal = new GenericPrincipal(HttpContext.User.Identity, new string[0]);
                CookieHelper.Instance.Role = loginResult.Role;
                return RedirectToLocal(returnUrl);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToPrevious();
        }
    }
}