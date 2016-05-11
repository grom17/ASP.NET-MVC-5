using ProjectsApp.Classes.Helpers;
using ProjectsApp.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace ProjectsApp.Controllers
{
    public class ProjectsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReadProjects()
        {
            try
            {
                var projects = DBHelper.Instance.GetProjectsList();
                return Content(GlobalHelper.Json(projects));
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ProjectDetails(int ProjectId)
        {
            try
            {
                List<EmployeeModel> employees = DBHelper.Instance.GetEmployeesList();
                var project = DBHelper.Instance.GetProjectById(ProjectId);
                ViewBag.EmployeesList = employees.Select(f => new SelectListItem { Text = f.Fullname, Value = f.PersonId.ToString() }).ToList();
                return PartialView("ProjectDetails", project);
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrepareCreateProject()
        {
            try
            {
                List<EmployeeModel> employees = DBHelper.Instance.GetEmployeesList();
                ViewBag.EmployeesList = employees.Select(f => new SelectListItem { Text = f.Fullname, Value = f.PersonId.ToString() }).ToList();
                return PartialView("NewProject", new ProjectModel());
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}