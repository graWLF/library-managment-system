using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Entities
{
    public class Branch
    {
        public int Id { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }

        public ICollection<Borrowing> Borrowings { get; set; }
    }
}
