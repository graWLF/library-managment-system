using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Entities
{
    [Table("isbn_authorid")]
    public class Isbnauthorid

    {
        [Column("isbn")]
        public long Id { get; set; }
        public long authorid { get; set; }

        //public ICollection<Isbnauthorid> isbnauthorids { get; set; }
    }
}
