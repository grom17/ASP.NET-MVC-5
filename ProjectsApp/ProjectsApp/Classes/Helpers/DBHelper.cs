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
                List<Staff> employees = new List<Staff>();
                using (ProjectsDB db = new ProjectsDB())
                {
                    employees = db.Staff.ToList();
                }
                return GetEmployeesList(employees);
            }
            catch (Exception ex)
            {
                throw new DBException("GetEmployeesList(): ", ex.ToString());
            }
        }

        public List<EmployeeModel> GetEmployeesList(List<Staff> employees)
        {
            var Mapper = MapperHelper.CreateMap<Staff, EmployeeModel>();
            return employees.ConvertAll(f => Mapper.Map<EmployeeModel>(f));
        }

        public EmployeeModel GetEmployeeById(int PersonId)
        {
            try
            {
                List<Staff> employees = new List<Staff>();
                using (ProjectsDB db = new ProjectsDB())
                {
                    employees = db.Staff.ToList();
                }
                return GetEmployeeById(employees, PersonId);           
            }
            catch (Exception ex)
            {
                throw new DBException("GetEmployeeById(): ", ex.ToString());
            }
        }

        public EmployeeModel GetEmployeeById(List<Staff> employees, int PersonId)
        {
            var employee = employees.Where(s => s.PersonId == PersonId).FirstOrDefault();
            if (employee == null)
                throw new Exception(string.Format(Messages.EmployeeNotExists, PersonId));
            var Mapper = MapperHelper.CreateMap<Staff, EmployeeModel>();
            return Mapper.Map<EmployeeModel>(employee);
        }

        public void UpdateEmployee(EmployeeModel model)
        {
            try
            {
                using (ProjectsDB db = new ProjectsDB())
                {
                    var employee = db.Staff.Where(s => s.PersonId == model.PersonId).FirstOrDefault();
                    employee.FirstName = model.FirstName;
                    employee.Patronymic = model.Patronymic;
                    employee.LastName = model.LastName;
                    employee.Email = model.Email;
                    db.Entry(employee).State = EntityState.Modified;
                    db.SaveChanges();
                }
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
            using (ProjectsDB db = new ProjectsDB())
            {
                db.Staff.Add(employee);
                db.SaveChanges();
                return db.Staff.Where(s => s.Email == model.Email).Select(s => s.PersonId).FirstOrDefault();
            }
        }

        public void DeleteEmployee(int PersonId)
        {
            using (ProjectsDB db = new ProjectsDB())
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
        }

        public List<ProjectModel> GetProjectsList()
        {
            try
            {
                List<ProjectInfo> projects = new List<ProjectInfo>();
                List<Staff> staff = new List<Staff>();
                List<ProjectExecutors> executors = new List<ProjectExecutors>();
                using (ProjectsDB db = new ProjectsDB())
                {
                    projects = db.ProjectInfo.ToList();
                    staff = db.Staff.ToList();
                    executors = db.ProjectExecutors.ToList();
                }
                return GetProjectsList(projects, staff, executors);
            }
            catch (Exception ex)
            {
                throw new DBException("GetProjectsList(): ", ex.ToString());
            }
        }

        public List<ProjectModel> GetProjectsList(List<ProjectInfo> projects, List<Staff> staff, List<ProjectExecutors> executors)
        {
            List<ProjectModel> projectsList = new List<ProjectModel>();
            projectsList = (from pi in projects
                            join st in staff
                            on pi.ProjectManagerId equals st.PersonId
                            group pi by pi.ProjectId into grp
                            select new ProjectModel
                            {
                                ProjectId = grp.Select(x => x.ProjectId).FirstOrDefault(),
                                ClientCompanyName = grp.Select(x => x.ClientCompanyName).FirstOrDefault(),
                                ExecutiveCompanyName = grp.Select(x => x.ExecutiveCompanyName).FirstOrDefault(),
                                StartDate = grp.Select(x => x.StartDate).FirstOrDefault(),
                                EndDate = grp.Select(x => x.EndDate).FirstOrDefault(),
                                Priority = grp.Select(x => x.Priority).FirstOrDefault(),
                                Comment = grp.Select(x => x.Comment).FirstOrDefault(),
                                ProjectManagerId = grp.Select(x => x.ProjectManagerId).FirstOrDefault(),
                                ProjectManagerName = grp.Select(x => x.Staff.Fullname).FirstOrDefault()
                            }).ToList();
            foreach (var pr in projectsList)
            {
                pr.ProjectExecutors = GetProjectExecutorsList(staff, executors, pr.ProjectId);
            }
            return projectsList;
        }

        public List<EmployeeModel> GetProjectExecutorsList(List<Staff> staff, List<ProjectExecutors> executors, int ProjectId)
        {
            try
            {
                List<EmployeeModel> projectExecutors = new List<EmployeeModel>();
                projectExecutors = (from pe in executors
                                    where pe.ProjectId == ProjectId
                                 join st in staff
                                 on pe.ProjectExecutorId equals st.PersonId
                                 group st by st.PersonId into grp
                                 select new EmployeeModel
                                 {
                                     PersonId = grp.Select(x => x.PersonId).FirstOrDefault(),
                                     FirstName = grp.Select(x => x.FirstName).FirstOrDefault(),
                                     Patronymic = grp.Select(x => x.Patronymic).FirstOrDefault(),
                                     LastName = grp.Select(x => x.LastName).FirstOrDefault(),
                                     Email = grp.Select(x => x.Email).FirstOrDefault()
                                 }).ToList();
                return projectExecutors;
            }
            catch (Exception ex)
            {
                throw new DBException("GetProjectExecutorsList(): ", ex.ToString());
            }
        }
    }
}