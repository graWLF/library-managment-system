using AutoMapper;
using CleanArchitecture.Core.DTOs.Borrowing;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services
{
    public class BorrowingService : IBorrowingService
    {
        private readonly IBorrowingRepository _repository;
        private readonly IMapper _mapper;

        public BorrowingService(IBorrowingRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BorrowingDTO>> GetAllAsync()
        {
            var borrowerings = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<BorrowingDTO>>(borrowerings);
        }

        public async Task<BorrowingDTO> GetByCompositeKeyAsync(long itemNo, long borrowerId, string borrowDate, string dueDate)
        {
            var entity = await _repository.GetByCompositeKeyAsync(itemNo, borrowerId, borrowDate, dueDate);
            return _mapper.Map<BorrowingDTO>(entity);
        }


        public async Task CreateAsync(BorrowingDTO dto)
        {
            var borrowing = _mapper.Map<Borrowing>(dto);
            await _repository.AddAsync(borrowing);
        }

        public async Task UpdateAsync(long itemNo, long borrowerId, string borrowDate, string dueDate, BorrowingDTO dto)
        {
            var existing = await _repository.GetByCompositeKeyAsync(itemNo, borrowerId, borrowDate, dueDate);
            if (existing == null) throw new Exception("Borrowing not found");

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
        }


        public async Task DeleteAsync(long itemNo, long borrowerId, string borrowDate, string dueDate)
        {
            var existing = await _repository.GetByCompositeKeyAsync(itemNo, borrowerId, borrowDate, dueDate);
            if (existing == null) throw new Exception("Borrowing not found");

            await _repository.DeleteAsync(existing);
        }

    }
}
