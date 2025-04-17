using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Entities
{
    public class Supervisor
    {
        public int Id { get; set; }
        public string SupervisorName { get; set; }
        public ICollection<Librarian> librarians { get; set; } // Related Librarian entities
    }
}
