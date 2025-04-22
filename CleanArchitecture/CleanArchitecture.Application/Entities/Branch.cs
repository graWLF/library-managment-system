using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Entities
{
    [Table("branch")]
    public class Branch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("branchid")]
        public int Id { get; set; }
        public string branchname { get; set; }
        public string branchaddress { get; set; }

        public ICollection<Borrowing> Borrowings { get; set; }
    }
}
