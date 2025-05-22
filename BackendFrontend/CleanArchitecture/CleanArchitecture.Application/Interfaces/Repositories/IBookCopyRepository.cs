using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities;

namespace CleanArchitecture.Core.Interfaces.Repositories
{
    public interface IBookCopyRepository
    {
        Task<IEnumerable<BookCopy>> GetAllAsync();
        Task<BookCopy> GetByIdAsync(long Id);
        Task AddAsync(BookCopy bookCopy);
        Task UpdateAsync(BookCopy bookCopy);
        Task DeleteAsync(BookCopy bookCopy);
        Task<IEnumerable<BookCopy>> GetAllByIsbnAsync(long Id);

    }
}
