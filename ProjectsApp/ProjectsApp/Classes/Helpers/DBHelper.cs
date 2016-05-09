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
       
        */
    }
}