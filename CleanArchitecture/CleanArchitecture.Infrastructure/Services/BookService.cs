using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.DTOs;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.DTOs.Book;

namespace CleanArchitecture.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly List<BookDto> _books = new();
        private int _nextId = 1;

        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            return await Task.FromResult(_books);
        }

        public async Task<BookDto> GetByIdAsync(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            return await Task.FromResult(book);
        }

        public async Task<BookDto> CreateAsync(BookDto bookDto)
        {
            bookDto.Id = _nextId++;
            _books.Add(bookDto);
            return await Task.FromResult(bookDto);
        }

        public async Task<BookDto> UpdateAsync(int id, BookDto bookDto)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return null;

            book.Title = bookDto.Title;
            book.Author = bookDto.Author;
            book.ISBN = bookDto.ISBN;
            book.PublishedOn = bookDto.PublishedOn;

            return await Task.FromResult(book);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return await Task.FromResult(false);
            _books.Remove(book);
            return await Task.FromResult(true);
        }
    }
}

