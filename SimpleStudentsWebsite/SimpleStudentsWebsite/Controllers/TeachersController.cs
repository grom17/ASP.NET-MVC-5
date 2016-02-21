using AutoMapper;
using SimpleStudentsWebsite.Classes;
using SimpleStudentsWebsite.Classes.Attributes;
using SimpleStudentsWebsite.Classes.Helpers;
using SimpleStudentsWebsite.DAL;
using SimpleStudentsWebsite.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SimpleStudentsWebsite.Controllers
{
    public class TeachersController : BaseController
    {
        // GET: Teachers
        [AllowAnonymous]
        public ActionResult Index()
        {          
            return View();
        }

        // GET: Teachers/ReadTeachers
        [AllowAnonymous]
        public ActionResult ReadTeachers()
        {
            List<TeacherModel> teachers = DBHelper.Instance.GetTeachersListWithStudentsCount();
            return Content(GlobalHelper.Json(teachers));
        }

        // GET: Teachers/TeacherDetails
        [Role(Access = Roles.Teacher | Roles.Dean)]
        public ActionResult TeacherDetails(int Id)
        {
            try
            {
                var teacher = DBHelper.Instance.GetTeacherById(Id);
                return PartialView("TeacherDetails", teacher);
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Teachers/LoadStudentGrades
        [Role(Access = Roles.Teacher | Roles.Dean)]
        public ActionResult LoadTeacherStudents(int Id)
        {
            try
            {
                var teacherStudents = DBHelper.Instance.GetTeacherStudentsList(Id);
                return Content(GlobalHelper.Json(teacherStudents));
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Teachers/UpdateTeacherDetails
        [Role(Access = Roles.Dean)]
        [HttpPost]
        public ActionResult UpdateTeacherDetails(Teachers teacher)
        {
            try
            {
                DBHelper.Instance.UpdateTeacher(teacher);
                return Json(new { success = "Профиль преподавателя обновлен" });
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message });
            }
        }

        // POST: Teachers/UpdateTeacherStudents
        [Role(Access = Roles.Dean)]
        [HttpPost]
        public ActionResult UpdateTeacherStudents(List<TeacherStudentsModel> students)
        {
            try
            {
                if (students == null)
                    throw new Exception("Студенты не выбраны");
                DBHelper.Instance.UpdateTeacherStudents(students);
                return Json(new { success = "Список студентов преподавателя обновлен" });
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message });
            }
        }

        // Get: Teachers/PrepareCreateTeacher
        [Role(Access = Roles.Dean)]
        public ActionResult PrepareCreateTeacher()
        {
            try
            {
                return PartialView("NewTeacher", new NewTeacher());
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Teachers/CreateTeacher
        [Role(Access = Roles.Dean)]
        [HttpPost]
        public ActionResult CreateTeacher(NewTeacher newTeacher)
        {
            try
            {
                CheckModelState(ModelState, "Teachers.CreateTeacher");
                newTeacher.Password = AESCrypt.EncryptString(newTeacher.Password, "SSWSecretKey");
                var Mapper = new MapperConfiguration(cfg => cfg.CreateMap<NewTeacher, Teachers>()).CreateMapper();
                Teachers teacher = Mapper.Map<Teachers>(newTeacher);
                var Id = DBHelper.Instance.CreateTeacher(teacher);
                return Json(new { success = "Преподаватель успешно добавлен", Id = Id });
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message });
            }
        }

        // GET: Teachers/DeleteTeacher
        [Role(Access = Roles.Dean)]
        public ActionResult DeleteTeacher(int Id)
        {
            try
            {
                DBHelper.Instance.DeleteTeacher(Id);
                return Json(new { success = "Преподаватель успешно удален" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}