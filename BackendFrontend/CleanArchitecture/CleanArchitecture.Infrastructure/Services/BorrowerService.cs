using AutoMapper;
using CleanArchitecture.Core.DTOs.Borrower;
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
    public class BorrowerService : IBorrowerService
    {
        private readonly IBorrowerRepository _repository;
        private readonly IMapper _mapper;

        public BorrowerService(IBorrowerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BorrowerDTO>> GetAllAsync()
        {
            var borrowers = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<BorrowerDTO>>(borrowers);
        }

        public async Task<BorrowerDTO> GetByIDAsync(long ID)
        {
            var borrower = await _repository.GetByIdAsync(ID);
            return _mapper.Map<BorrowerDTO>(borrower);
        }

        public async Task CreateAsync(BorrowerDTO dto)
        {
            var borrower = _mapper.Map<Borrower>(dto);
            await _repository.AddAsync(borrower);
        }

        public async Task UpdateAsync(long ID, BorrowerDTO dto)
        {
            var existing = await _repository.GetByIdAsync(ID);
            if (existing == null) throw new Exception("Borrower not found");

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(long ID)
        {
            var borrower = await _repository.GetByIdAsync(ID);
            if (borrower == null) throw new Exception("Borrower not found");

            await _repository.DeleteAsync(borrower);
        }
    }
}
