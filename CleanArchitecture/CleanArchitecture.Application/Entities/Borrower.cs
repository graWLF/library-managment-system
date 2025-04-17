using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Entities
{
    public class Borrower
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Roles? Role { get; set; }

        public ICollection<Borrowing> Borrowings { get; set; }
    }
}
