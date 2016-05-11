using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectsApp.Models
{
    public class ProjectModel
    {
        public int ProjectId { get; set; }

        public int ProjectManagerId { get; set; }

        public string ProjectManagerName { get; set; }

        [Required]
        [StringLength(50)]
        public string ExecutiveCompanyName { get; set; }

        [Required]
        [StringLength(50)]
        public string ClientCompanyName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Priority { get; set; }

        public string Comment { get; set; }

        //public List<EmployeeModel> ProjectExecutors { get; set; }

        public string StartDateDisplay
        {
            get
            {
                return StartDate.ToShortDateString();
            }
        }

        public string EndDateDisplay
        {
            get
            {
                return EndDate.ToShortDateString();
            }
        }
    }
}