using SimpleStudentsWebsite.Classes.Helpers;
using System.Linq;
using System.Web.Mvc;

namespace SimpleStudentsWebsite.Controllers
{
    public class StudentsController : BaseController
    {
        // GET: Students
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Students/ReadStudents
        [AllowAnonymous]
        public ActionResult ReadStudents()
        {
            var students = DBHelper.Instance.GetStudentsList().AsEnumerable();
            return PartialView("StudentsList", students);

            //object[] students = new object[2];
            //students[0] = new object[] { "Ivan", 5 };
            //students[1] = new object[] { "Igor", 6 };
            //return Content(GlobalHelper.Json(students));
        }
    }
}