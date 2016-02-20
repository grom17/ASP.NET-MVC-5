namespace SimpleStudentsWebsite.Models
{
    public class TeacherStudentsModel
    {
        public int TeacherId { get; set; }
        public int StudentId { get; set; }
        public bool IsStudent { get; set; }
        public string StudentFullName { get; set; }
    }
}