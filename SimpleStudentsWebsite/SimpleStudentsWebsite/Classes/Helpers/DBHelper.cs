using SimpleStudentsWebsite.Classes.Exceptions;
using SimpleStudentsWebsite.DAL;
using SimpleStudentsWebsite.Models;
using SimpleStudentsWebsite.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleStudentsWebsite.Classes.Helpers
{
    public class DBHelper
    {
        private SSW_DB_Model db = new SSW_DB_Model();
        private static DBHelper mInstance = null;
        public static DBHelper Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new DBHelper();
                return mInstance;
            }
        }

        // Get user login info
        public LoginModel Login(string Username, string Password)
        {
            try
            {
                // Encrypt password before compare with its DB value
                string encryptedPassword = AESCrypt.EncryptString(Password, "SSWSecretKey");

                // get dean with specific username and password
                var d = (from dn in db.Deans
                         where dn.Login.Equals(Username) && dn.Password.Equals(encryptedPassword)
                         select dn).FirstOrDefault();

                // if dean exists and password is correct return his fullname, login and role=Dean
                if (d != null)
                    return PrepareLoginModel(d.Fullname, d.Login, Roles.Dean);

                // if dean was not found, get teacher with specific username and password
                var t = (from tch in db.Teachers
                         where tch.Login.Equals(Username) && tch.Password.Equals(encryptedPassword)
                         select tch).FirstOrDefault();

                // if teacher exists and password is correct return his fullname, login and role=Teacher
                if (t != null)
                    return PrepareLoginModel(t.Fullname, t.Login, Roles.Teacher);

                // if dean and teacher was not found, get student with specific username and password
                var s = (from std in db.Students
                         where std.Login.Equals(Username) && std.Password.Equals(encryptedPassword)
                         select std).FirstOrDefault();

                // if student exists and password is correct return his fullname, login and role=Student
                if (s != null)
                    return PrepareLoginModel(s.Fullname, s.Login, Roles.Student);

                // If no one user was not found in DB return error
                throw new Exception("Неверный логин или пароль");
            }
            catch (Exception ex)
            {
                throw new DBException("Login(): ", ex.Message);
            }
        }

        // Prepare LoginModel and get login result
        public LoginModel PrepareLoginModel(string Fullname, string Login, Roles Role)
        {
            LoginModel loginModel = new LoginModel()
            { 
                Fullname = Fullname,
                Role = Role,
                Login = Login
            };
            return loginModel;
        }

        // Get students array with average grade
        public StudentModel[] GetStudentsArray()
        {
            var students = (from std in db.Students
                     join jr in db.Journal
                     on std.StudentId equals jr.StudentId
                     group jr by std.StudentId into grp
                     select new StudentModel
                     {
                         Id = grp.Select(st => st.Students.StudentId).FirstOrDefault(),
                         Fullname = grp.Select(st => st.Students.FirstName + " " + st.Students.LastName).FirstOrDefault(),
                         //Fullname = grp.Select(st => st.Students.Fullname).FirstOrDefault(),
                         Grades = grp.Where(g => g.Grade.HasValue && g.Grade.Value > 0).Select(g => g.Grade.Value).Average()
                     }).ToArray();
            if (students != null)
                return students;
            throw new Exception("Ошибка получения списка студентов");
        }

        // Get teachers array with students count
        public TeacherModel[] GetTeachersArray()
        {
            var teachers = (from tch in db.Teachers
                            join jr in db.Journal
                            on tch.TeacherId equals jr.TeacherId
                            group jr by tch.TeacherId into grp
                            select new TeacherModel
                            {
                                Id = grp.Select(x => x.Teachers.TeacherId).FirstOrDefault(),
                                Fullname = grp.Select(tc => tc.Teachers.FirstName + " " + tc.Teachers.LastName).FirstOrDefault(),
                                StudentsCount = grp.Count()
                            }).ToArray();
            if (teachers != null)
                return teachers;
            throw new Exception("Ошибка получения списка преподователей");
        }

        // Get students with average grades list
        public List<StudentModel> GetStudentsList()
        {
            try
            {
                return GetStudentsArray().ToList();
            }
            catch (Exception ex)
            {
                throw new DBException("GetStudentsList(): ", ex.ToString());
            }        
        }

        // Get best students (student average grade is more or equal total average grade)
        public List<StudentModel> GetBestStudentsList()
        {
            try
            {
                return GetStudentsArray()
                        .Where(x => x.Grades >= db.Journal.Select(g => g.Grade).Average()).ToList();
            }
            catch (Exception ex)
            {
                throw new DBException("GetBestStudentsList(): ", ex.ToString());
            }
        }

        // Get all teachers list
        public List<TeacherModel> GetTeachersList()
        {
            try
            {
                return GetTeachersArray().ToList();
            }
            catch (Exception ex)
            {
                throw new DBException("GetTeachersList(): ", ex.ToString());
            }
        }

        // Get only teachers of all students
        public List<TeacherModel> GetTeachersOfAllStudentsList()
        {
            try
            {
                return GetTeachersArray().Where(x => x.StudentsCount == db.Students.Count()).ToList();
            }
            catch (Exception ex)
            {
                throw new DBException("GetTeachersOfAllStudentsList(): ", ex.ToString());
            }
        }

        // Get only teachers of lower count of students
        public List<TeacherModel> GetTeachersOfLowerCountOfStudentsList()
        {
            try
            {
                var averageCountOfStudents = GetTeachersArray().Select(x => x.StudentsCount).Average();
                return GetTeachersArray().Where(x => x.StudentsCount < averageCountOfStudents).ToList();
            }
            catch (Exception ex)
            {
                throw new DBException("GetTeachersOfLowerCountOfStudentsList(): ", ex.ToString());
            }
        }
    }
}