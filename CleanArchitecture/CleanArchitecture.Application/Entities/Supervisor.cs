using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Entities
{
    [Table("supervisor")]
    public class Supervisor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("supervisorid")]
        public int Id { get; set; }
        public string supervisorname { get; set; }
        public ICollection<Librarian> librarians { get; set; } // Related Librarian entities
    }
}
