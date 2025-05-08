using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Entities
{
    [Table("book_info")]
    public class Book
    {
        [Key]
        [Column("isbn")]
        
        public long Id { get; set; }
        public string local_isbn { get; set; }   
        public string type { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public string additiondate { get; set; }
        public string content { get; set; }
        public string infourl { get; set; }
        public string contentlanguage { get; set; }
        public string contentsource { get; set; }
        public string image { get; set; }
        public string price { get; set; }
        public string duration { get; set; }
        public string contentlink { get; set; }
        public int librarianid { get; set; }
        public string format { get; set; }
        public string publishingstatus { get; set; }
        public string releasedate { get; set; }
        public int publisherid { get; set; }
        public int pages { get; set; }
        public string weight { get; set; }
        public string dimensions { get; set; }
        public string material { get; set; }
        public string color { get; set; }
        
    }
}
