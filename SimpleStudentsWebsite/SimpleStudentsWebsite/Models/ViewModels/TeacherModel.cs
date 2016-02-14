using System.ComponentModel.DataAnnotations;

namespace SimpleStudentsWebsite.Models.ViewModels
{
    public class TeacherModel : PersonModel
    {
        [Display(Name = "Количество студентов")]
        public int StudentsCount { get; set; }
    }
}