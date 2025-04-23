using CleanArchitecture.Core.DTOs.Book;
using CleanArchitecture.Core.DTOs.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IRegistrationService
    {
        Task<IEnumerable<RegistrationDTO>> GetAllAsync();
        Task<RegistrationDTO> GetByIDAsync(int ID);
        Task CreateAsync(RegistrationDTO dto);
        Task UpdateAsync(int ID, RegistrationDTO dto);
        Task DeleteAsync(int ID);
    }
}
