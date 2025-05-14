using AutoMapper;
using CleanArchitecture.Core.DTOs.Publisher;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository _repository;
        private readonly IMapper _mapper;

        public PublisherService(IPublisherRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PublisherDTO>> GetAllAsync()
        {
            var publishers = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<PublisherDTO>>(publishers);
        }

        public async Task<PublisherDTO> GetByIDAsync(long ID)
        {
            var publisher = await _repository.GetByIDAsync(ID);
            return _mapper.Map<PublisherDTO>(publisher);
        }

        public async Task CreateAsync(PublisherDTO dto)
        {
            var publisher = _mapper.Map<Publisher>(dto);
            await _repository.AddAsync(publisher);
        }

        public async Task UpdateAsync(long ID, PublisherDTO dto)
        {
            var existing = await _repository.GetByIDAsync(ID);
            if (existing == null) throw new Exception("Publisher not found");

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(long ID)
        {
            var publisher = await _repository.GetByIDAsync(ID);
            if (publisher == null) throw new Exception("Publisher not found");

            await _repository.DeleteAsync(publisher);
        }
    }
}
