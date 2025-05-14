using CleanArchitecture.Core.DTOs.Librarian;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface ILibrarianService
    {
        Task<IEnumerable<LibrarianDTO>> GetAllAsync();
        Task<LibrarianDTO> GetByIDAsync(int ID);
        Task CreateAsync(LibrarianDTO dto);
        Task UpdateAsync(int ID, LibrarianDTO dto);
        Task DeleteAsync(int ID);
    }
}
