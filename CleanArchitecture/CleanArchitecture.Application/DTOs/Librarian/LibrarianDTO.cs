using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.DTOs.Librarian
{
    public class LibrarianDTO
    {
        public int Id { get; set; }
        public int SupervisorId { get; set; }
        public string LibrarianName { get; set; }
    }
}
