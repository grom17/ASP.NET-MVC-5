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
            try
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
            catch (Exception ex)
            {
                throw new DBException("CreateEmployee(): ", ex.ToString());
            }
        }

        public void DeleteEmployee(int PersonId)
        {
            try
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
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DBException("DeleteEmployee(): ", ex.ToString());
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

        public ProjectModel GetProjectById(int ProjectId)
        {
            try
            {
                List<ProjectInfo> projects = new List<ProjectInfo>();
                using (ProjectsDB db = new ProjectsDB())
                {
                    projects = db.ProjectInfo.ToList();
                }
                return GetProjectById(projects, ProjectId);
            }
            catch (Exception ex)
            {
                throw new DBException("GetProjectById(): ", ex.ToString());
            }
        }

        public ProjectModel GetProjectById(List<ProjectInfo> projects, int ProjectId)
        {
            var project = projects.Where(s => s.ProjectId == ProjectId).FirstOrDefault();
            if (project == null)
                throw new Exception(string.Format(Messages.ProjectNotExists, ProjectId));
            var Mapper = MapperHelper.CreateMap<ProjectInfo, ProjectModel>();
            return Mapper.Map<ProjectModel>(project);
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
            return projectsList;
        }

        public List<EmployeeModel> GetProjectExecutorsList(int ProjectId)
        {
            try
            {
                List<Staff> staff = new List<Staff>();
                List<ProjectExecutors> executors = new List<ProjectExecutors>();
                using (ProjectsDB db = new ProjectsDB())
                {
                    staff = db.Staff.ToList();
                    executors = db.ProjectExecutors.ToList();
                }
                return GetProjectExecutorsList(staff, executors, ProjectId);
            }
            catch (Exception ex)
            {
                throw new DBException("GetProjectExecutorsList(): ", ex.ToString());
            }
        }

        public List<EmployeeModel> GetProjectExecutorsList(List<Staff> staff, List<ProjectExecutors> executors, int ProjectId)
        {
            try
            {
                List<EmployeeModel> employees = new List<EmployeeModel>();
                employees = (from pe in executors
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
                return employees;
            }
            catch (Exception ex)
            {
                throw new DBException("GetProjectExecutorsList(): ", ex.ToString());
            }
        }

        public void AddProjectExecutor(int ProjectId, int PersonId)
        {
            try
            {
                ProjectExecutors executor = new ProjectExecutors { ProjectId = ProjectId, ProjectExecutorId = PersonId };
                using (ProjectsDB db = new ProjectsDB())
                {
                    db.ProjectExecutors.Add(executor);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DBException("AddProjectExecutor(): ", ex.ToString());
            }
        }

        public bool ValidateProjectExecutor(int ProjectId, int PersonId)
        {
            try
            {
                using (ProjectsDB db = new ProjectsDB())
                {
                    return db.ProjectExecutors.Where(x => x.ProjectExecutorId == PersonId && x.ProjectId == ProjectId).Select(x => x.Id).FirstOrDefault() == 0;
                }
            }
            catch (Exception ex)
            {
                throw new DBException("ValidateProjectExecutor(): ", ex.ToString());
            }
        }

        public void DeleteProjectExecutor(int ProjectId, int PersonId)
        {
            try
            {
                using (ProjectsDB db = new ProjectsDB())
                {
                    var executor = db.ProjectExecutors.Where(x => x.ProjectId == ProjectId && x.ProjectExecutorId == PersonId).FirstOrDefault();
                    if (executor != null)
                    {
                        db.Entry(executor).State = EntityState.Deleted;
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DBException("DeleteProjectExecutor(): ", ex.ToString());
            }
        }

        public int CreateProject(ProjectModel model)
        {
            try
            {
                var Mapper = MapperHelper.CreateMap<ProjectModel, ProjectInfo>();
                var project = Mapper.Map<ProjectInfo>(model);
                using (ProjectsDB db = new ProjectsDB())
                {
                    db.ProjectInfo.Add(project);
                    db.SaveChanges();
                    int Id = db.ProjectInfo.Where(p => p.ClientCompanyName == model.ClientCompanyName && p.StartDate == model.StartDate).Select(s => s.ProjectId).FirstOrDefault();
                    AddProjectExecutor(Id, model.ProjectManagerId);
                    return Id;
                }
            }
            catch (Exception ex)
            {
                throw new DBException("CreateProject(): ", ex.ToString());
            }
        }

        public void UpdateProject(ProjectModel model)
        {
            try
            {
                using (ProjectsDB db = new ProjectsDB())
                {
                    var project = db.ProjectInfo.Where(p => p.ProjectId == model.ProjectId).FirstOrDefault();
                    project.ClientCompanyName = model.ClientCompanyName;
                    project.ExecutiveCompanyName = model.ExecutiveCompanyName;
                    project.Priority = model.Priority;
                    project.StartDate = model.StartDate;
                    project.EndDate = model.EndDate;
                    project.Comment = model.Comment;
                    project.ProjectManagerId = model.ProjectManagerId;
                    db.Entry(project).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DBException("UpdateProject(): ", ex.ToString());
            }
        }

        public void DeleteProject(int ProjectId)
        {
            try
            {
                using (ProjectsDB db = new ProjectsDB())
                {
                    var project = db.ProjectInfo.Where(p => p.ProjectId == ProjectId).FirstOrDefault();
                    if (project != null)
                    {
                        db.Entry(project).State = EntityState.Deleted;
                        // Removing dependencies 
                        foreach (var pe in db.ProjectExecutors.ToList())
                        {
                            if (pe.ProjectId == ProjectId)
                            {
                                db.ProjectExecutors.Remove(pe);
                            }
                        }
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DBException("DeleteProject(): ", ex.ToString());
            }
        }
    }
}