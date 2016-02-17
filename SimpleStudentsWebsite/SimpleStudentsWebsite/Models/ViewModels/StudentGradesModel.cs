using SimpleStudentsWebsite.Classes.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SimpleStudentsWebsite.Models.ViewModels
{
    public class StudentGradesModel
    {
        [CustomDisplayName("StudentId")]
        public int StudentId { get; set; }
        [CustomDisplayName("TeacherId")]
        public int TeacherId { get; set; }
        [CustomDisplayName("TeacherFullName")]
        public string TeacherFullName { get; set; }
        [CustomDisplayName("Subject")]
        public string Subject { get; set; }
        [CustomDisplayName("Grade")]
        public int? Grade { get; set; }
    }
}