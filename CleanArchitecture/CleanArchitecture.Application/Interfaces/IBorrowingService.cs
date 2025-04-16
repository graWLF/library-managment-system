using CleanArchitecture.Core.DTOs.Borrowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IBorrowingService
    {
        Task<IEnumerable<BorrowingDTO>> GetAllAsync();
        Task<BorrowingDTO> GetByIDAsync(long ID);
        Task CreateAsync(BorrowingDTO dto);
        Task UpdateAsync(long ID, BorrowingDTO dto);
        Task DeleteAsync(long ID);
    }
}
