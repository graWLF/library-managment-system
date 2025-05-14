using CleanArchitecture.Core.DTOs.Branch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IBranchService
    {
        Task<IEnumerable<BranchDTO>> GetAllAsync();
        Task<BranchDTO> GetByIDAsync(int ID);
        Task CreateAsync(BranchDTO dto);
        Task UpdateAsync(int ID, BranchDTO dto);
        Task DeleteAsync(int ID);
    }
}
