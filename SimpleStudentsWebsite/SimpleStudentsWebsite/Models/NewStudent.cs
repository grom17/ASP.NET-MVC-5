using SimpleStudentsWebsite.Classes.Attributes;
using SimpleStudentsWebsite.DAL;
using System.ComponentModel.DataAnnotations;
namespace SimpleStudentsWebsite.Models
{
    public class  NewStudent : Students
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} ������ ���� �� ����� {2} �������� ������", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [CustomDisplayName("Password")]
        public new string Password { get; set; }

        [DataType(DataType.Password)]
        [CustomDisplayName("ConfirmPassword")]
        [Compare("Password", ErrorMessage = "������ �� ���������")]
        public string ConfirmPassword { get; set; }
    }
}
