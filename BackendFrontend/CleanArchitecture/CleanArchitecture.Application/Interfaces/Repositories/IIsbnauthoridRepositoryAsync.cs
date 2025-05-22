using CleanArchitecture.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.Repositories
{
    public interface IIsbnauthoridRepository
    {
        Task<IEnumerable<Isbnauthorid>> GetAllAsync();
        Task<Isbnauthorid> GetByCompositeKeyAsync(long Id, long authorid);
        Task AddAsync(Isbnauthorid isbnauthorid);
        Task UpdateAsync(Isbnauthorid isbnauthorid);
        Task DeleteAsync(Isbnauthorid isbnauthorid);
        Task<IEnumerable<Isbnauthorid>> GetByISBNAsync(long Id);
    }
}
