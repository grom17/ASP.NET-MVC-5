using ProjectsApp.Classes.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ProjectsApp.Models
{
    public class EmployeeModel
    {
        public int PersonId { get; set; }

        [Required]
        [StringLength(50)]
        [CustomDisplayName("FirstName")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [CustomDisplayName("Patronymic")]
        public string Patronymic { get; set; }

        [Required]
        [StringLength(50)]
        [CustomDisplayName("LastName")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [CustomDisplayName("Email")]
        public string Email { get; set; }
    }
}