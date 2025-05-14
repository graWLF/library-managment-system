using CleanArchitecture.Core.DTOs.Supervisor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface ISupervisorService
    {
        Task<IEnumerable<SupervisorDTO>> GetAllAsync();
        Task<SupervisorDTO> GetByIDAsync(int ID);
        Task CreateAsync(SupervisorDTO dto);
        Task UpdateAsync(int ID, SupervisorDTO dto);
        Task DeleteAsync(int ID);
    }
}
