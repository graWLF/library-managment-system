using CleanArchitecture.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> GetByISBNAsync(long isbn);
        Task<IEnumerable<Book>> GetByNameAsync(string name);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(Book book);
    }
}
