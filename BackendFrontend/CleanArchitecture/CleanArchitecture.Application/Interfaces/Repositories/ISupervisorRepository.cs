using CleanArchitecture.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.Repositories
{
    public interface ISupervisorRepository
    {
        Task<IEnumerable<Supervisor>> GetAllAsync();
        Task<Supervisor> GetByIDAsync(int SupervisorID);
        Task AddAsync(Supervisor supervisor);
        Task UpdateAsync(Supervisor supervisor);
        Task DeleteAsync(Supervisor supervisor);
    }
}
