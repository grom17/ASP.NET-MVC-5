using System.ComponentModel.DataAnnotations;

namespace SimpleStudentsWebsite.Models
{
    public class StudentModel : PersonModel
    {
        [Display(Name = "Средний балл")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public double Grades { get; set; }
    }
}