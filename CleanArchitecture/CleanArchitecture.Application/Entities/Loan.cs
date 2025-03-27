using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Entities
{
    class Loan
    {
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime LoanedOn { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnedOn { get; set; }
    }
}
