using ProjectsApp.Classes.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectsApp.Models
{
    public class ProjectModel
    {
        public int ProjectId { get; set; }

        [CustomDisplayName("ProjectManagerName")]
        public int ProjectManagerId { get; set; }

        public string ProjectManagerName { get; set; }

        [Required]
        [StringLength(50)]
        [CustomDisplayName("ExecutiveCompanyName")]
        public string ExecutiveCompanyName { get; set; }

        [Required]
        [StringLength(50)]
        [CustomDisplayName("ClientCompanyName")]
        public string ClientCompanyName { get; set; }

        [CustomDisplayName("StartDate")]
        public DateTime StartDate { get; set; }

        [CustomDisplayName("EndDate")]
        public DateTime EndDate { get; set; }

        [Range(0,10)]
        [CustomDisplayName("Priority")]
        public int Priority { get; set; }

        [CustomDisplayName("Comment")]
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

        public int NewExecutorId { get; set; }
    }
}