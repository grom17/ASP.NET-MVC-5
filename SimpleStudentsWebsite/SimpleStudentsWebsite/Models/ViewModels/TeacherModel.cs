using System.ComponentModel.DataAnnotations;

namespace SimpleStudentsWebsite.Models.ViewModels
{
    public class TeacherModel : PersonModel
    {
        [Display(Name = "Кол-во студентов")]
        public int StudentsCount { get; set; }
    }
}