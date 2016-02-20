using SimpleStudentsWebsite.Classes.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SimpleStudentsWebsite.Models
{
    public class StudentGradesModel
    {
        public int StudentId { get; set; }
        public int TeacherId { get; set; }
        public bool IsTeacher { get; set; }
        [CustomDisplayName("TeacherFullName")]
        public string TeacherFullName { get; set; }
        [CustomDisplayName("Subject")]
        public string Subject { get; set; }
        [CustomDisplayName("Grade")]
        public int? Grade { get; set; }
    }
}