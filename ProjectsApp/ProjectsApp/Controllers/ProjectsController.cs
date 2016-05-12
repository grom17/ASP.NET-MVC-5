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
                project.NewExecutorId = employees.Where(x => x.PersonId != project.ProjectManagerId).Select(x => x.PersonId).FirstOrDefault();
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
                return PartialView("NewProject", new ProjectModel() { StartDate = DateTime.Today, EndDate = DateTime.Today });
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CreateProject(ProjectModel newProject)
        {
            try
            {
                var Id = DBHelper.Instance.CreateProject(newProject);
                return Json(new { success = Messages.ProjectCreatedSuccessfully, Id = Id });
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message });
            }
        }

        public ActionResult LoadProjectExecutors(int ProjectId)
        {
            try
            {
                var projectExecutors = DBHelper.Instance.GetProjectExecutorsList(ProjectId);
                return Content(GlobalHelper.Json(projectExecutors));
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddProjectExecutor(int ProjectId, int PersonId)
        {
            try
            {
                if (DBHelper.Instance.ValidateProjectExecutor(ProjectId, PersonId))
                {
                    DBHelper.Instance.AddProjectExecutor(ProjectId, PersonId);
                    return Json(new { success = Messages.ProjectDetailsUpdatedSuccessfully, NeedUpdate = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteProjectExecutor(int ProjectId, int PersonId)
        {
            try
            {
                DBHelper.Instance.DeleteProjectExecutor(ProjectId, PersonId);
                return Json(new { success = Messages.ProjectDetailsUpdatedSuccessfully, NeedUpdate = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateProject(ProjectModel model)
        {
            try
            {
                DBHelper.Instance.UpdateProject(model);
                if (DBHelper.Instance.ValidateProjectExecutor(model.ProjectId, model.ProjectManagerId))
                {
                    DBHelper.Instance.AddProjectExecutor(model.ProjectId, model.ProjectManagerId);
                }
                return Json(new { success = Messages.ProjectDetailsUpdatedSuccessfully });
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message });
            }
        }

        public ActionResult DeleteProject(int ProjectId)
        {
            try
            {
                DBHelper.Instance.DeleteProject(ProjectId);
                return Json(new { success = Messages.ProjectDeletedSuccessfully }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}