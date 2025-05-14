using AutoMapper;
using CleanArchitecture.Core.DTOs.Supervisor;
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
    public class SupervisorService : ISupervisorService
    {
        private readonly ISupervisorRepository _repository;
        private readonly IMapper _mapper;

        public SupervisorService(ISupervisorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SupervisorDTO>> GetAllAsync()
        {
            var supervisors = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<SupervisorDTO>>(supervisors);
        }

        public async Task<SupervisorDTO> GetByIDAsync(int ID)
        {
            var supervisor = await _repository.GetByIDAsync(ID);
            return _mapper.Map<SupervisorDTO>(supervisor);
        }

        public async Task CreateAsync(SupervisorDTO dto)
        {
            var supervisor = _mapper.Map<Supervisor>(dto);
            await _repository.AddAsync(supervisor);
        }

        public async Task UpdateAsync(int ID, SupervisorDTO dto)
        {
            var existing = await _repository.GetByIDAsync(ID);
            if (existing == null) throw new Exception("Supervisor not found");

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int ID)
        {
            var supervisor = await _repository.GetByIDAsync(ID);
            if (supervisor == null) throw new Exception("Supervisor not found");

            await _repository.DeleteAsync(supervisor);
        }
    }
}
