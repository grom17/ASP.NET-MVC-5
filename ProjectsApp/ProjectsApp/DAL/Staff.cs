namespace ProjectsApp.DAL
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Staff")]
    public partial class Staff
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Staff()
        {
            ProjectExecutors = new HashSet<ProjectExecutors>();
            ProjectInfo = new HashSet<ProjectInfo>();
        }

        [Key]
        public int PersonId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Patronymic { get; set; }

        public string Fullname
        {
            get
            {
                return FirstName.Substring(0, 1) + '.' + Patronymic.Substring(0, 1) + '.' + LastName;
            }
        }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectExecutors> ProjectExecutors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectInfo> ProjectInfo { get; set; }
    }
}
