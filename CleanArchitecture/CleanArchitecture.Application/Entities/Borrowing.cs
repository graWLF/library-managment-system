using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Entities
{
    public class Borrowing
    {
        public long Id { get; set; }
        public int BorrowerId { get; set; }
        public int BranchId { get; set; }
        public String BorrowDate { get; set; }
        public String DueDate { get; set; }
        public Boolean ReturnStatus { get; set; }

        public Borrower Borrower { get; set; } // Related Borrower entity
        public Branch Branch { get; set; }
    }


    
}
