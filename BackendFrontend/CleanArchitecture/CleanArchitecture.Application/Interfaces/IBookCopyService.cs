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
        Task<BookCopyDTO> GetByCompositeKeyAsync(long itemNo, long isbn);
        Task CreateAsync(BookCopyDTO dto);
        Task UpdateAsync(long itemNo, long copyNo, BookCopyDTO dto);
        Task DeleteAsync(long itemNo, long copyNo);
    }
}
