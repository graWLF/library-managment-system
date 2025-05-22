using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.DTOs.BookCopy;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IBookCopyService
    {
        Task<IEnumerable<BookCopyDTO>> GetAllAsync();
        Task<BookCopyDTO> GetByIdAsync(long Id);
        Task CreateAsync(BookCopyDTO dto);
        Task UpdateAsync(long Id, BookCopyDTO dto);
        Task DeleteAsync(long Id);
        Task DeleteByIsbnAsync(long Isbn);
    }
}
