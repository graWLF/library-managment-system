using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Entities
{
    [Table("borrower")]
    public class Borrower
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("borrowerid")]
        public long Id { get; set; }
        public string borrowername { get; set; }
        public string borrowerphone { get; set; }
        

        public ICollection<Borrowing> Borrowings { get; set; }
    }
}
