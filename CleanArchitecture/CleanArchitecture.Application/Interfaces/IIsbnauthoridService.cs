using CleanArchitecture.Core.DTOs.Isbnauthorid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IIsbnauthoridService
    {
        Task<IEnumerable<IsbnauthoridDTO>> GetAllAsync();
        Task<IsbnauthoridDTO?> GetByCompositeKeyAsync(long Id, long authorid);
        Task CreateAsync(IsbnauthoridDTO dto);
        Task UpdateAsync(long Id, long authorid, IsbnauthoridDTO dto);
        Task DeleteAsync(long Id, long authorid);
    }
}
