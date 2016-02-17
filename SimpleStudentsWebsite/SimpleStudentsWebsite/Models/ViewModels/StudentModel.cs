using System.ComponentModel.DataAnnotations;

namespace SimpleStudentsWebsite.Models.ViewModels
{
    public class StudentModel : PersonModel
    {
        [Display(Name = "Средний балл")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public double Grades { get; set; }
    }
}