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
        Task<BorrowingDTO?> GetByCompositeKeyAsync(long itemNo, long borrowerId, string borrowDate, string dueDate);
        Task CreateAsync(BorrowingDTO dto);
        Task UpdateAsync(long itemNo, long borrowerId, string borrowDate, string dueDate, BorrowingDTO dto);
        Task DeleteAsync(long itemNo, long borrowerId, string borrowDate, string dueDate);
    }
}
