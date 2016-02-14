using System.ComponentModel.DataAnnotations;

namespace SimpleStudentsWebsite.Models.ViewModels
{
    public class PersonModel
    {
        public int Id { get; set; }

        [Display(Name = "Полное имя")]
        public string Fullname { get; set; }
    }
}