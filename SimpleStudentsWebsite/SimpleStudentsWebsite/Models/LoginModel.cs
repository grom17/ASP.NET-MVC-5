using SimpleStudentsWebsite.Classes;

namespace SimpleStudentsWebsite.Models
{
    public class LoginModel
    {
        public string Fullname { get; set; }

        public string Login { get; set; }

        public Roles Role { get; set; }
    }
}