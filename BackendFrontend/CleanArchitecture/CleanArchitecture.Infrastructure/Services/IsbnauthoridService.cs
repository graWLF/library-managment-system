using AutoMapper;
using CleanArchitecture.Core.DTOs.Isbnauthorid;
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
    public class IsbnauthoridService : IIsbnauthoridService
    {
        private readonly IIsbnauthoridRepository _repository;
        private readonly IMapper _mapper;

        public IsbnauthoridService(IIsbnauthoridRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IsbnauthoridDTO>> GetByISBNAsync(long Id)
        {
            var borrowering = await _repository.GetByISBNAsync(Id);
            return _mapper.Map<IEnumerable<IsbnauthoridDTO>>(borrowering);
        }

        public async Task<IEnumerable<IsbnauthoridDTO>> GetAllAsync()
        {
            var borrowerings = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<IsbnauthoridDTO>>(borrowerings);
        }

        public async Task<IsbnauthoridDTO> GetByCompositeKeyAsync(long Id, long authorid)
        {
            var entity = await _repository.GetByCompositeKeyAsync(Id, authorid);
            return _mapper.Map<IsbnauthoridDTO>(entity);
        }


        public async Task CreateAsync(IsbnauthoridDTO dto)
        {
            var isbnauthorid = _mapper.Map<Isbnauthorid>(dto);
            await _repository.AddAsync(isbnauthorid);
        }

        public async Task UpdateAsync(long Id, long authorid, IsbnauthoridDTO dto)
        {
            var existing = await _repository.GetByCompositeKeyAsync(Id, authorid);
            if (existing == null) throw new Exception("Isbnauthorid not found");

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
        }


        public async Task DeleteAsync(long Id, long authorid)
        {
            var existing = await _repository.GetByCompositeKeyAsync(Id, authorid);
            if (existing == null) throw new Exception("Isbnauthorid not found");

            await _repository.DeleteAsync(existing);
        }

    }
}
