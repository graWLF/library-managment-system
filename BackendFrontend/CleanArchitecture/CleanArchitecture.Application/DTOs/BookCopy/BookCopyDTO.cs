using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.DTOs.BookCopy
{
    public class BookCopyDTO
    {
        public long Id { get; set; }
        public long Isbn { get; set; }
        public string Location { get; set; }
    }
}
