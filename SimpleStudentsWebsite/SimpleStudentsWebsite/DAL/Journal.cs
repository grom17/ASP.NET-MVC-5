using System.ComponentModel.DataAnnotations.Schema;
namespace SimpleStudentsWebsite.DAL
{
    [Table("Journal")]
    public partial class Journal
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public int TeacherId { get; set; }

        public int? Grade { get; set; }

        public virtual Students Students { get; set; }

        public virtual Teachers Teachers { get; set; }
    }
}
