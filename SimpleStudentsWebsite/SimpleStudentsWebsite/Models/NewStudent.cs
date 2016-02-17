using SimpleStudentsWebsite.Classes.Attributes;
using SimpleStudentsWebsite.DAL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleStudentsWebsite.Models
{
    [NotMapped]
    public class  NewStudent : Students
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} должен быть не менее {2} символов длиной", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [CustomDisplayName("Password")]
        public string SecretKey { get; set; }

        [DataType(DataType.Password)]
        [CustomDisplayName("ConfirmPassword")]
        [Compare("SecretKey", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
