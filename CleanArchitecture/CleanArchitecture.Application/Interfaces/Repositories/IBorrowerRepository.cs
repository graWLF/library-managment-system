using CleanArchitecture.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.Repositories
{
    public interface IBorrowerRepository
    {
        Task<IEnumerable<Borrower>> GetAllAsync();
        Task<Borrower> GetByIDAsync(int id);
        Task AddAsync(Borrower borrower);
        Task UpdateAsync(Borrower borrower);
        Task DeleteAsync(Borrower borrower);
    }
}
