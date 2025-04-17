using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Entities
{
    public class Book
    {
        public long ID { get; set; }
        public string LocalISBN { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string AdditionDate { get; set; }
        public string Content { get; set; }
        public string InfoUrl { get; set; }
        public string ContentLanguage { get; set; }
        public string ContentSource { get; set; }
        public string Image { get; set; }
        public string Price { get; set; }
        public string Duration { get; set; }
        public string ContentLink { get; set; }
        public int LibrarianId { get; set; }
        public string Format { get; set; }
        public string PublisingStatus { get; set; }
        public string ReleaseDate { get; set; }
        public int PublisherId { get; set; }
        public int Pages { get; set; }
        public string Weight { get; set; }
        public string Dimensions { get; set; }
        public string Material { get; set; }
        public string Color { get; set; }
        
    }
}
