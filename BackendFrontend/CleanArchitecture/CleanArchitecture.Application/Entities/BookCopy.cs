using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Entities
{
    [Table("isbn_itemno")]
    public class BookCopy
    {
        [Column("itemno")]
        [Key]
        public long Id { get; set; }
        public long isbn { get; set; }
        public string location { get; set; }

        [ForeignKey("isbn")]
        public Book Book { get; set; } // Navigation property to the Book entity

        public ICollection<Borrowing> Borrowings { get; set; } // Navigation property to Borrowing
    }
}
