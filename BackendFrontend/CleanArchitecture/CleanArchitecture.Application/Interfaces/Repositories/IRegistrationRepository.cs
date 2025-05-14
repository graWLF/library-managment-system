using CleanArchitecture.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.Repositories
{
    public interface IRegistrationRepository
    {
        Task<IEnumerable<Registration>> GetAllAsync();
        Task<Registration> GetByIDAsync(int id);
        Task AddAsync(Registration registration);
        Task UpdateAsync(Registration registration);
        Task DeleteAsync(Registration registration);
        bool Login(string username, string password);
    }
}
