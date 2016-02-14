using System.ComponentModel.DataAnnotations;

namespace SimpleStudentsWebsite.Models.ViewModels
{
    public class StudentModel : PersonModel
    {
        [Display(Name = "Средний балл")]
        public double Grades { get; set; }
    }
}