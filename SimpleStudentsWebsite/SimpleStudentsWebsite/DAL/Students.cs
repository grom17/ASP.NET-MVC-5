using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SimpleStudentsWebsite.DAL
{
    public partial class Students : Person
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Students()
        {
            Journal = new HashSet<Journal>();
        }

        [Key]
        public int StudentId { get; set; }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Journal> Journal { get; set; }
    }
}
