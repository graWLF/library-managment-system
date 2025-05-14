using CleanArchitecture.Core.DTOs.Author;
using CleanArchitecture.Core.DTOs.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDTO>> GetAllAsync();
        Task<AuthorDTO> GetByIDAsync(int ID);
        Task CreateAsync(AuthorDTO dto);
        Task UpdateAsync(int ID, AuthorDTO dto);
        Task DeleteAsync(int ID);
    }
}
