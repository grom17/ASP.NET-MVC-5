using SimpleStudentsWebsite.Classes.Helpers;
using System.ComponentModel.DataAnnotations;
namespace SimpleStudentsWebsite.DAL
{
    public partial class Person
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Login { get; set; }

        [Required]
        [StringLength(200)]
        public string Password { get; set; }

        public string Fullname
        {
            get
            {
                return GlobalHelper.GetFullname(FirstName, LastName);
            }
        }
    }
}
