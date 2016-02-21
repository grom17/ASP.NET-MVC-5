using SimpleStudentsWebsite.Classes.Attributes;
using SimpleStudentsWebsite.DAL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleStudentsWebsite.Models
{
    [NotMapped]
    public class  NewStudent : Students
    {
        [DataType(DataType.Password)]
        [CustomDisplayName("ConfirmPassword")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
