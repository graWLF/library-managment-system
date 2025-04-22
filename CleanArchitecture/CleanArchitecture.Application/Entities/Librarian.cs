using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Entities
{
    [Table("librarian")]
    public class Librarian
    {
        [Key]
        [Column("librarianid")]
        public int Id { get; set; }
        
        public int supervisorid { get; set; }
        public string librarianname { get; set; }

        public Supervisor supervisor { get; set; } // Related Supervisor entity
    }
}
