using CleanArchitecture.Core.DTOs.Publisher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IPublisherService
    {
        Task<IEnumerable<PublisherDTO>> GetAllAsync();
        Task<PublisherDTO> GetByIDAsync(long ID);
        Task CreateAsync(PublisherDTO dto);
        Task UpdateAsync(long ID, PublisherDTO dto);
        Task DeleteAsync(long ID);
    }
}
