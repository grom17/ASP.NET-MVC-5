using System.Web.Mvc;
using SimpleStudentsWebsite.Models;
using SimpleStudentsWebsite.Classes.Helpers;
using System.Threading;
using System.Security.Principal;
using System.Web.Security;
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
            try
            {
                CheckModelState(ModelState, "Account.Login");

                LoginModel loginResult = DBHelper.Instance.Login(model.Login, model.Password);
                var ck = FormsAuthentication.GetAuthCookie(loginResult.Fullname, false);
                HttpContext.Response.Cookies.Add(ck);
                HttpContext.User = Thread.CurrentPrincipal = new GenericPrincipal(HttpContext.User.Identity, new string[0]);
                CookieHelper.Instance.Role = loginResult.Role;
                string url = "";
                if (loginResult.Role == Classes.Roles.Student)
                    url = Url.Action("Index", "Students");
                else if (loginResult.Role == Classes.Roles.Teacher)
                    url = Url.Action("Index", "Teachers");
                else if (loginResult.Role == Classes.Roles.Dean)
                    url = Url.Action("Index", "Reports");
                else
                    url = Url.Action("Index", "Home");
                if (!string.IsNullOrEmpty(returnUrl))
                    url = returnUrl;
                return Json(new { url = url });
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message });
                //ModelState.AddModelError("", ex.Message);
                //return View(model);
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
    }
}