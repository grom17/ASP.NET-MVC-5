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

        public List<ProjectModel> GetProjectsList()
        {
            try
            {
                List<ProjectModel> projects = new List<ProjectModel>();
                projects = (from pi in db.ProjectInfo
                                join st in db.Staff
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
                                    ProjectManagerName = grp.Select(x => x.Staff.FirstName).FirstOrDefault() + " " +
                                                         grp.Select(x => x.Staff.Patronymic).FirstOrDefault() + " " +
                                                         grp.Select(x => x.Staff.LastName).FirstOrDefault()                                                                 
                                }).ToList();
                foreach (var pr in projects)
                {
                    pr.ProjectManagerName = GetFullname(pr.ProjectManagerName);
                    pr.ProjectExecutors = GetProjectExecutorsList(pr.ProjectId);
                }
                return projects;
            }
            catch (Exception ex)
            {
                throw new DBException("GetProjectsList(): ", ex.ToString());
            }
        }

        public string GetFullname(string name)
        {
            string[] names = name.Split();
            string FirstName = names[0];
            string Patronymic = names[1];
            string LastName = names[2];
            return FirstName.Substring(0, 1) + '.' + Patronymic.Substring(0, 1) + '.' + LastName;
        }

        public List<EmployeeModel> GetProjectExecutorsList(int ProjectId)
        {
            try
            {
                List<EmployeeModel> projectExecutors = new List<EmployeeModel>();
                projectExecutors = (from pe in db.ProjectExecutors
                                 where pe.ProjectId == ProjectId
                                 join st in db.Staff
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