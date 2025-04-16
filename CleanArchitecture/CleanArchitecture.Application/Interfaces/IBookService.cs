using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.DTOs.Book;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllAsync();
        Task<BookDto> GetByISBNAsync(long isbn);
        Task CreateAsync(BookDto dto);
        Task UpdateAsync(long isbn, BookDto dto);
        Task DeleteAsync(long isbn);
    }
}
