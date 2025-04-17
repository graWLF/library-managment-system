using AutoMapper;
using CleanArchitecture.Core.DTOs.Branch;
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
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _repository;
        private readonly IMapper _mapper;

        public BranchService(IBranchRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BranchDTO>> GetAllAsync()
        {
            var branches = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<BranchDTO>>(branches);
        }

        public async Task<BranchDTO> GetByIDAsync(int ID)
        {
            var branch = await _repository.GetByIDAsync(ID);
            return _mapper.Map<BranchDTO>(branch);
        }

        public async Task CreateAsync(BranchDTO dto)
        {
            var branch = _mapper.Map<Branch>(dto);
            await _repository.AddAsync(branch);
        }

        public async Task UpdateAsync(int ID, BranchDTO dto)
        {
            var existing = await _repository.GetByIDAsync(ID);
            if (existing == null) throw new Exception("branch not found");

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int ID)
        {
            var branch = await _repository.GetByIDAsync(ID);
            if (branch == null) throw new Exception("Branch not found");

            await _repository.DeleteAsync(branch);
        }
    }
}
