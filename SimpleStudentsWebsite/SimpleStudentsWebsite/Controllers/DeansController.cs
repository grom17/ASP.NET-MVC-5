using SimpleStudentsWebsite.Classes.Attributes;
using SimpleStudentsWebsite.Models.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SimpleStudentsWebsite.Controllers
{
    [Role(Access = Classes.Roles.Dean)]
    public class DeansController : BaseController
    {
        // GET: Deans
        public ActionResult Index()
        {
            List<ReportModel> reports = new List<ReportModel>()
            {
                new ReportModel() { Id = 1, ReportName = "Лучшие студенты" },
                new ReportModel() { Id = 2, ReportName = "Преподаватели, обучающие всех студентов" },
                new ReportModel() { Id = 3, ReportName = "Преподаватели с наименьшим количеством студентов" }
            };
            return View(reports);
        }
    }
}