using System;


namespace CleanArchitecture.Core.DTOs.Book
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishedOn { get; set; }
        public string Description { get; set; }
    }
}
