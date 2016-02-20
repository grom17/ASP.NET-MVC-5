using System.ComponentModel.DataAnnotations;

namespace SimpleStudentsWebsite.Models
{
    public class TeacherModel : PersonModel
    {
        [Display(Name = "Кол-во студентов")]
        public int StudentsCount { get; set; }
    }
}