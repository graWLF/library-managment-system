using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.DTOs.Book;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllAsync();
        Task<BookDto> GetByIdAsync(int id);
        Task<BookDto> CreateAsync(BookDto bookDto);
        Task<BookDto> UpdateAsync(int id, BookDto bookDto);
        Task<bool> DeleteAsync(int id);
    }
}
