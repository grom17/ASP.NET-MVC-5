namespace ProjectsApp.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProjectInfo")]
    public partial class ProjectInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProjectInfo()
        {
            ProjectExecutors = new HashSet<ProjectExecutors>();
        }

        [Key]
        public int ProjectId { get; set; }

        public int ProjectManagerId { get; set; }

        [Required]
        [StringLength(50)]
        public string ExecutiveCompanyName { get; set; }

        [Required]
        [StringLength(50)]
        public string ClientCompanyName { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        public int Priority { get; set; }

        [Column(TypeName = "ntext")]
        public string Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectExecutors> ProjectExecutors { get; set; }

        public virtual Staff Staff { get; set; }
    }
}
