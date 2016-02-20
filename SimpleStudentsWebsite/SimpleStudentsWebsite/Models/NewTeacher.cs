using SimpleStudentsWebsite.Classes.Attributes;
using SimpleStudentsWebsite.DAL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleStudentsWebsite.Models
{
    [NotMapped]
    public class NewTeacher : Teachers
    {
        [DataType(DataType.Password)]
        [CustomDisplayName("ConfirmPassword")]
        [Compare("Password", ErrorMessage = "������ �� ���������")]
        public string ConfirmPassword { get; set; }
    }
}
