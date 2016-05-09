using SimpleStudentsWebsite.Classes.Exceptions;
using ProjectsApp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using ProjectsApp.Models;
using System.Data.Entity;

namespace ProjectsApp.Classes.Helpers
{
    public class DBHelper
    {
        private ProjectsDB db = new ProjectsDB();
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

        public List<EmployeeModel> GetEmployeesList()
        {
            try
            {
                var employees = db.Staff.ToList();
                var Mapper = MapperHelper.CreateMap<Staff, EmployeeModel>();
                return employees.ConvertAll(f => Mapper.Map<EmployeeModel>(f));
            }
            catch (Exception ex)
            {
                throw new DBException("GetEmployeesList(): ", ex.ToString());
            }
        }

        public EmployeeModel GetEmployeeById(int PersonId)
        {
            try
            {
                var employee = db.Staff.Where(s => s.PersonId == PersonId).FirstOrDefault();
                if (employee == null)
                    throw new Exception(string.Format(Messages.EmployeeNotExists, PersonId));
                var Mapper = MapperHelper.CreateMap<Staff, EmployeeModel>();
                return Mapper.Map<EmployeeModel>(employee);
            }
            catch (Exception ex)
            {
                throw new DBException("GetEmployeeById(): ", ex.ToString());
            }
        }

        public void UpdateEmployee(EmployeeModel model)
        {
            try
            {
                var employee = db.Staff.Where(s => s.PersonId == model.PersonId).FirstOrDefault();
                employee.FirstName = model.FirstName;
                employee.Patronymic = model.Patronymic;
                employee.LastName = model.LastName;
                employee.Email = model.Email;
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DBException("UpdateEmployee(): ", ex.ToString());
            }
        }

        public int CreateEmployee(EmployeeModel model)
        {
            var Mapper = MapperHelper.CreateMap<EmployeeModel, Staff>();
            var employee = Mapper.Map<Staff>(model);
            db.Staff.Add(employee);
            db.SaveChanges();
            return db.Staff.Where(s => s.Email == model.Email).Select(s => s.PersonId).FirstOrDefault();
        }

        public void DeleteEmployee(int PersonId)
        {
            var employee = db.Staff.Where(s => s.PersonId == PersonId).FirstOrDefault();
            if (employee != null)
            {
                db.Entry(employee).State = EntityState.Deleted;
                // Removing all dependencies 
                foreach (var pe in db.ProjectExecutors.ToList())
                {
                    if (pe.ProjectExecutorId == PersonId)
                    {
                        db.ProjectExecutors.Remove(pe);
                    }
                }
                foreach (var pi in db.ProjectInfo.ToList())
                {
                    if (pi.ProjectManagerId == PersonId)
                    {
                        db.ProjectInfo.Remove(pi);
                    }
                }
            }
            db.SaveChanges();
        }
        /*
        // Get teachers list with students count
        public List<TeacherModel> GetTeachersListWithStudentsCount()
        {
            try
            {
                List<TeacherModel> teachers = new List<TeacherModel>();
                // Select all teachers from journal
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

        // Get teachers without students
        public List<Teachers> GetFreeTeachers()
        {
            return db.Teachers
                    .Where(s => !db.Journal
                    .Select(j => j.TeacherId)
                    .Contains(s.TeacherId)).ToList();
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

        public List<StudentGradesModel> GetStudentGradesList(int Id)
        {
            try
            {
                // List of teachers
                List<Teachers>  teachers = db.Teachers.ToList();
                // List of teachers without students
                List<Teachers> freeTeachers = GetFreeTeachers();
                // List of grades
                List<Journal> journal = db.Journal.ToList();
                return GetStudentGradesList(teachers, freeTeachers, journal, Id);
            }
            catch (Exception ex)
            {
                throw new DBException("GetStudentGradesList(): ", ex.ToString());
            }
        }

        // Get student grades list
        // contains all teachers/subjects but marked by IsTeacher flag for current student
        // to allow to attach student to teacher(s) or remove
        public List<StudentGradesModel> GetStudentGradesList(List<Teachers> teachers, List<Teachers> freeTeachers, List<Journal> journal, int Id)
        {
            List<StudentGradesModel> grades = new List<StudentGradesModel>();
            // For the new student get list of all teachers (not from journal), IsTeacher is false for all
            if (Id == 0)
            {
                grades = (from th in teachers
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
                // Select all teachers from journal
                grades = (from tch in teachers
                          join jr in journal
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
                // Include teachers without any students in list
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

        // Get students belongs to no one teacher
        public List<Students> GetFreeStudents()
        {
            return db.Students
                        .Where(s => !db.Journal
                        .Select(j => j.StudentId)
                        .Contains(s.StudentId)).ToList();
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
        */
    }
}