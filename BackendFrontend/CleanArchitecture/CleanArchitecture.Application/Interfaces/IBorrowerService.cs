﻿using CleanArchitecture.Core.DTOs.Book;
using CleanArchitecture.Core.DTOs.Borrower;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IBorrowerService
    {
        Task<IEnumerable<BorrowerDTO>> GetAllAsync();
        Task<BorrowerDTO> GetByIDAsync(long ID);
        Task CreateAsync(BorrowerDTO dto);
        Task UpdateAsync(long ID, BorrowerDTO dto);
        Task DeleteAsync(long ID);
    }
}
