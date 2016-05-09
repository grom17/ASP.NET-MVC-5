using System.Web.Mvc;
using ProjectsApp.Classes.Helpers;
using System;
using ProjectsApp.Models;

namespace ProjectsApp.Controllers
{
    public class StaffController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReadEmployees()
        {
            try
            {
                var employees = DBHelper.Instance.GetEmployeesList();
                return Content(GlobalHelper.Json(employees));
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EmployeeDetails(int PersonId)
        {
            try
            {
                var employee = DBHelper.Instance.GetEmployeeById(PersonId);
                return PartialView("EmployeeDetails", employee);
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateEmployee(EmployeeModel model)
        {
            try
            {
                DBHelper.Instance.UpdateEmployee(model);
                return Json(new { success = Messages.EmployeeDetailsUpdatedSuccessfully });
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message });
            }
        }

        public ActionResult DeleteEmployee(int PersonId)
        {
            try
            {
                DBHelper.Instance.DeleteEmployee(PersonId);
                return Json(new { success = Messages.EmployeeDeletedSuccessfully }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrepareCreateEmployee()
        {
            try
            {
                return PartialView("NewEmployee", new EmployeeModel());
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CreateEmployee(EmployeeModel newEmployee)
        {
            try
            {
                var Id = DBHelper.Instance.CreateEmployee(newEmployee);
                return Json(new { success = Messages.EmployeeCreatedSuccessfully, Id = Id });
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message });
            }
        }
    }
}