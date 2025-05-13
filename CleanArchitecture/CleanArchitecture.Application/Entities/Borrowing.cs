using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Entities
{
    [Table("borrowing")]
    public class Borrowing
    {
        [Key]
        [Column("itemno")]
        public long Id { get; set; }
        public long borrowerid { get; set; }
        public int branchid { get; set; }
        public String borrowdate { get; set; }
        public String duedate { get; set; }
        public Boolean returnstatus { get; set; }

        public Borrower Borrower { get; set; } // Related Borrower entity
        public Branch Branch { get; set; }
    }


    
}
