using CleanArchitecture.Core.DTOs.Librarian;
using CleanArchitecture.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.Repositories
{
    public interface ILibrarianRepository
    {
        Task<IEnumerable<Librarian>> GetAllAsync();
        Task<Librarian> GetByIDAsync(int LibrarianID);
        Task AddAsync(Librarian librarian);
        Task UpdateAsync(Librarian librarian);
        Task DeleteAsync(Librarian librarian);
    }
}
