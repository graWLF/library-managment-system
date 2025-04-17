using CleanArchitecture.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.Repositories
{
    public interface IBorrowingRepository
    {
        Task<IEnumerable<Borrowing>> GetAllAsync();
        Task<Borrowing> GetByIDAsync(long id);
        Task AddAsync(Borrowing borrowing);
        Task UpdateAsync(Borrowing borrowing);
        Task DeleteAsync(Borrowing borrowing);
    }
}
