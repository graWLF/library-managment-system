using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.DTOs.Borrower
{
    public class BorrowerDTO
    {
        public long Id { get; set; }
        public string borrowerName { get; set; }
        public string borrowerPhone { get; set; }
        
    }
}
