using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Entities
{
    public class Librarian
    {
        public int LibrarianId { get; set; }
        public int SupervisorId { get; set; }
        public string LibrarianName { get; set; }
    }
}
