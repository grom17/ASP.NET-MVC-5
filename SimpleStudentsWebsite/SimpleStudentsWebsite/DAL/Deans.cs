using System.ComponentModel.DataAnnotations;
namespace SimpleStudentsWebsite.DAL
{
    public partial class Deans : Person
    {
        [Key]
        public int DeanId { get; set; }
    }
}
