using SimpleStudentsWebsite.Classes.Exceptions;
using SimpleStudentsWebsite.DAL;
using SimpleStudentsWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        // Get students list with average grade
        public List<StudentModel> GetStudentsListWithAverageGrade()
        {
            try
            {
                var students = (from std in db.Students
                                join jr in db.Journal
                                on std.StudentId equals jr.StudentId
                                group jr by std.StudentId into grp
                                select new StudentModel
                                {
                                    Id = grp.Select(st => st.Students.StudentId).FirstOrDefault(),
                                    Fullname = grp.Select(st => st.Students.LastName + " " + st.Students.FirstName).FirstOrDefault(),
                                    NullableGrades = grp.Where(g => g.Grade.HasValue && g.Grade.Value > 0).Select(g => g.Grade.Value).Average()
                                }).ToList();
                // Find students without any teacher and include in list
                var freeStudents = GetFreeStudents();
                freeStudents.ForEach(x => students.Add(new StudentModel()
                {                   
                    Id = x.StudentId,
                    Fullname = x.LastName + " " + x.FirstName
                }));
                if (students != null)
                    return students;
                return new List<StudentModel>();
            }
            catch (Exception ex)
            {
                throw new DBException("GetStudentsListWithAverageGrade(): ", ex.ToString());
            }
        }

        // Get teachers list with students count
        public List<TeacherModel> GetTeachersListWithStudentsCount()
        {
            try
            {
                List<TeacherModel> teachers = new List<TeacherModel>();
                teachers = (from tch in db.Teachers
                            join jr in db.Journal
                            on tch.TeacherId equals jr.TeacherId
                            group jr by tch.TeacherId into grp
                            select new TeacherModel
                            {
                                Id = grp.Select(x => x.Teachers.TeacherId).FirstOrDefault(),
                                Fullname = grp.Select(tc => tc.Teachers.LastName + " " + tc.Teachers.FirstName).FirstOrDefault(),
                                StudentsCount = grp.Count()
                            }).ToList();
                // Find teachers without any student and include in list
                var freeTeachers = GetFreeTeachers();
                freeTeachers.ForEach(x => teachers.Add(new TeacherModel()
                {
                    Id = x.TeacherId,
                    StudentsCount = 0,
                    Fullname = x.LastName + " " + x.FirstName
                }));
                if (teachers != null)
                    return teachers;
                return new List<TeacherModel>();
            }
            catch (Exception ex)
            {
                throw new DBException("GetTeachersList(): ", ex.ToString());
            }
        }

        public List<Teachers> GetFreeTeachers()
        {
            return db.Teachers
                    .Where(s => !db.Journal
                    .Select(j => j.TeacherId)
                    .Contains(s.TeacherId)).ToList();
        }

        // Get student details
        public Students GetStudentById(int Id)
        {
            try
            {
                var student = db.Students.Where(s => s.StudentId == Id).FirstOrDefault();
                if (student == null)
                    throw new Exception(string.Format("Студента с Id={0} нет в базе", Id));
                return student;
            }
            catch (Exception ex)
            {
                throw new DBException("GetStudentById(): ", ex.ToString());
            }
        }

        // Get best students (student average grade is more or equal total average grade)
        public List<StudentModel> GetBestStudentsList()
        {
            try
            {
                var students = GetStudentsListWithAverageGrade();
                if (students.Count > 0)
                    return students.Where(x => x.Grades >= db.Journal.Select(g => g.Grade).Average()).ToList();
                return students;
            }
            catch (Exception ex)
            {
                throw new DBException("GetBestStudentsList(): ", ex.ToString());
            }
        }

        // Get only teachers of all students
        public List<TeacherModel> GetTeachersOfAllStudentsList()
        {
            try
            {
                return GetTeachersListWithStudentsCount().Where(x => x.StudentsCount == db.Students.Count()).ToList();
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
                var teachers = GetTeachersListWithStudentsCount();
                if (teachers != null && teachers.Count > 0)
                {
                    var averageCountOfStudents = teachers.Select(x => x.StudentsCount).Average();
                    return teachers.Where(x => x.StudentsCount < averageCountOfStudents).ToList();
                }
                return new List<TeacherModel>();
            }
            catch (Exception ex)
            {
                throw new DBException("GetTeachersOfLowerCountOfStudentsList(): ", ex.ToString());
            }
        }

        // Get student grades list
        // contains all teachers/subjects but marked by IsTeacher flag for current student
        // to allow to attach student to teacher(s) or remove
        public List<StudentGradesModel> GetStudentGradesList(int Id)
        {
            try
            {
                List<StudentGradesModel> grades = new List<StudentGradesModel>();
                // For the new student get list of all teachers (not from journal), IsTeacher is false for all
                if (Id == 0)
                {
                    grades = (from th in db.Teachers
                              select new StudentGradesModel
                              {
                                  StudentId = 0,
                                  TeacherId = th.TeacherId,
                                  TeacherFullName = th.LastName + " " + th.FirstName,
                                  IsTeacher = false,
                                  Subject = th.Subject,
                                  Grade = 0
                              }).ToList();
                }
                else
                {
                    grades = (from tch in db.Teachers
                              join jr in db.Journal
                              on tch.TeacherId equals jr.TeacherId
                              group jr by tch.TeacherId into grp
                              select new StudentGradesModel
                              {
                                  StudentId = Id,
                                  TeacherId = grp.Select(x => x.Teachers.TeacherId).FirstOrDefault(),
                                  TeacherFullName = grp.Select(tc => tc.Teachers.LastName + " " + tc.Teachers.FirstName).FirstOrDefault(),
                                  IsTeacher = grp.Where(x => x.StudentId.Equals(Id)).Select(x => x.StudentId).FirstOrDefault() == Id,
                                  Subject = grp.Select(s => s.Teachers.Subject).FirstOrDefault(),
                                  Grade = grp.Where(x => x.StudentId.Equals(Id)).Select(x => x.Grade).FirstOrDefault()
                              }).ToList();
                    // Find teachers without any students and include in list
                    var freeTeachers = GetFreeTeachers();
                    freeTeachers.ForEach(x => grades.Add(new StudentGradesModel()
                    {
                        StudentId = Id,
                        TeacherId = x.TeacherId,
                        TeacherFullName = x.LastName + " " + x.FirstName,
                        IsTeacher = false,
                        Subject = x.Subject,
                        Grade = 0
                    }));
                }
                if (grades != null)
                    return grades;
                return new List<StudentGradesModel>();
            }
            catch (Exception ex)
            {
                throw new DBException("GetStudentGradesList(): ", ex.ToString());
            }
        }

        // Get teacher students list
        // contains all students but marked by IsStudent flag for current teacher
        // to allow to attach student(s) to teacher or remove
        public List<TeacherStudentsModel> GetTeacherStudentsList(int Id)
        {
            try
            {
                List<TeacherStudentsModel> students = new List<TeacherStudentsModel>();
                // For the new teacher get list of all students (not from journal), IsStudent is false for all
                if (Id == 0)
                {
                    students = (from std in db.Students
                                select new TeacherStudentsModel
                                {
                                    TeacherId = Id,
                                    StudentId = std.StudentId,
                                    StudentFullName = std.LastName + " " + std.FirstName,
                                    IsStudent = false
                                }).ToList();
                }
                else
                {
                    students = (from std in db.Students
                                join jr in db.Journal
                                on std.StudentId equals jr.StudentId
                                group jr by std.StudentId into grp
                                select new TeacherStudentsModel
                                {
                                    TeacherId = Id,
                                    StudentId = grp.Select(x => x.Students.StudentId).FirstOrDefault(),
                                    StudentFullName = grp.Select(tc => tc.Students.LastName + " " + tc.Students.FirstName).FirstOrDefault(),
                                    IsStudent = grp.Where(x => x.TeacherId.Equals(Id)).Select(x => x.TeacherId).FirstOrDefault() == Id
                                }).ToList();
                    // Find students without any teacher and include in list
                    var freeStudents = GetFreeStudents();
                    freeStudents.ForEach(x => students.Add(new TeacherStudentsModel()
                    {
                        TeacherId = Id, IsStudent = false,
                        StudentId = x.StudentId, StudentFullName = x.LastName + " " + x.FirstName
                    }));
                }
                if (students != null)
                    return students;
                return new List<TeacherStudentsModel>();
            }
            catch (Exception ex)
            {
                throw new DBException("GetTeacherStudentsList(): ", ex.ToString());
            }
        }

        public List<Students> GetFreeStudents()
        {
            return db.Students
                        .Where(s => !db.Journal
                        .Select(j => j.StudentId)
                        .Contains(s.StudentId)).ToList();
        }

        public void UpdateStudent(Students model)
        {
            try
            {
                var student = db.Students.Where(s => s.StudentId == model.StudentId).FirstOrDefault();
                student.FirstName = model.FirstName;
                student.LastName = model.LastName;
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DBException("UpdateStudent(): ", ex.ToString());
            }
        }

        public void UpdateTeacher(Teachers model)
        {
            try
            {
                var teacher = db.Teachers.Where(t => t.TeacherId == model.TeacherId).FirstOrDefault();
                teacher.FirstName = model.FirstName;
                teacher.LastName = model.LastName;
                teacher.Subject = model.Subject;
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DBException("UpdateTeacher(): ", ex.ToString());
            }
        }

        public void UpdateStudentGrades(List<StudentGradesModel> model)
        {
            try
            {
                int teacherId = 0;
                int studentId = model[0].StudentId;
                for (int i = 0; i < model.Count; i++)
                {
                    teacherId = model[i].TeacherId;
                    // If student is no longer belongs to the current teacher, remove record from Journal
                    if (!model[i].IsTeacher)
                    {
                        var record = db.Journal.Where(jr => jr.TeacherId == teacherId &&
                                         jr.StudentId == studentId).FirstOrDefault();

                        // record found - remove
                        if (record != null)
                        {
                            db.Entry(record).State = EntityState.Deleted;
                        }
                    }
                    // Student belongs to the current teacher
                    else
                    {
                        var record = db.Journal.Where(jr => jr.TeacherId == teacherId &&
                                         jr.StudentId == studentId).FirstOrDefault();

                        // record found - update grade
                        if (record != null)
                        {
                            record.Grade = model[i].Grade;
                            db.Entry(record).State = EntityState.Modified;
                        }
                        // need to add new record
                        else
                        {
                            record = new Journal()
                            {
                                StudentId = studentId,
                                TeacherId = teacherId,
                                Grade = model[i].Grade
                            };
                            record.Teachers = db.Teachers.Where(t => t.TeacherId == teacherId).FirstOrDefault();
                            record.Students = db.Students.Where(s => s.StudentId == studentId).FirstOrDefault();
                            db.Journal.Add(record);
                        }
                    }
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DBException("UpdateStudentGrades(): ", ex.ToString());
            }
        }

        // Update journal, add/remove teacher students
        public void UpdateTeacherStudents(List<TeacherStudentsModel> model)
        {
            try
            {
                int studentId = 0;
                int teacherId = model[0].TeacherId;
                for (int i = 0; i < model.Count; i++)
                {
                    studentId = model[i].StudentId;
                    // If student is no longer belongs to the current teacher, remove record from Journal
                    if (!model[i].IsStudent)
                    {
                        var record = db.Journal.Where(jr => jr.StudentId == studentId &&
                                         jr.TeacherId == teacherId).FirstOrDefault();

                        // record found - remove
                        if (record != null)
                        {
                            db.Entry(record).State = EntityState.Deleted;
                        }
                    }
                    // Student belongs to the current teacher
                    else
                    {
                        var record = db.Journal.Where(jr => jr.TeacherId == teacherId &&
                                         jr.StudentId == studentId).FirstOrDefault();

                        // record not found - need to add new record
                        if (record == null)
                        {
                            record = new Journal()
                            {
                                StudentId = studentId,
                                TeacherId = teacherId,
                                Grade = 0
                            };
                            record.Teachers = db.Teachers.Where(t => t.TeacherId == teacherId).FirstOrDefault();
                            record.Students = db.Students.Where(s => s.StudentId == studentId).FirstOrDefault();
                            db.Journal.Add(record);
                        }
                    }
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DBException("UpdateTeacherStudents(): ", ex.ToString());
            }
        }

        // Get teachers
        public List<Teachers> GetTeachers()
        {
            return db.Teachers.ToList();
        }

        // Get students
        public List<Students> GetStudents()
        {
            return db.Students.ToList();
        }

        // Get teacher details
        public Teachers GetTeacherById(int Id)
        {
            try
            {
                var teacher = db.Teachers.Where(t => t.TeacherId == Id).FirstOrDefault();
                if (teacher == null)
                    throw new Exception(string.Format("Преподавателя с Id={0} нет в базе", Id));
                return teacher;
            }
            catch (Exception ex)
            {
                throw new DBException("GetTeacherById(): ", ex.ToString());
            }
        }

        // Create new student
        public int CreateStudent(Students student)
        {
            db.Students.Add(student);
            db.SaveChanges();
            return db.Students.Where(s => s.Login == student.Login).Select(s => s.StudentId).FirstOrDefault();
        }

        // Create new teacher
        public int CreateTeacher(Teachers teacher)
        {
            db.Teachers.Add(teacher);
            db.SaveChanges();
            return db.Teachers.Where(t => t.Login == teacher.Login).Select(t => t.TeacherId).FirstOrDefault();
        }

        // Remove student from DB by Id
        public void DeleteStudent(int Id)
        {
            var student = db.Students.Where(s => s.StudentId == Id).FirstOrDefault();
            if (student != null)
            {
                db.Entry(student).State = EntityState.Deleted;
                foreach (var jr in db.Journal.Local.ToList())
                {
                    if (jr.Students == null)
                    {
                        db.Journal.Remove(jr);
                    }
                }
            }
            db.SaveChanges();
        }

        // Remove teacher from DB by Id
        public void DeleteTeacher(int Id)
        {
            var teacher = db.Teachers.Where(t => t.TeacherId == Id).FirstOrDefault();
            if (teacher != null)
            {
                db.Entry(teacher).State = EntityState.Deleted;
                foreach (var jr in db.Journal.Local.ToList())
                {
                    if (jr.Teachers == null)
                    {
                        db.Journal.Remove(jr);
                    }
                }
            }
            db.SaveChanges();
        }
    }
}