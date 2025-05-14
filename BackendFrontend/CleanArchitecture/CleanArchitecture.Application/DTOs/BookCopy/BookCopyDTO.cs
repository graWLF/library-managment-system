using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.DTOs.BookCopy
{
    public class BookCopyDTO
    {
        public long itemno { get; set; }
        public long isbn { get; set; }
        public string location { get; set; }
    }
}
