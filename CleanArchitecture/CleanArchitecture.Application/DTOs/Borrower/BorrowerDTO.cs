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
        public int BorrowerId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Roles? Role { get; set; }
    }
}
