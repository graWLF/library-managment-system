using AutoMapper;
using CleanArchitecture.Core.DTOs.Author;
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
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _repository;
        private readonly IMapper _mapper;

        public AuthorService(IAuthorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AuthorDTO>> GetAllAsync()
        {
            var authors = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<AuthorDTO>>(authors);
        }

        public async Task<AuthorDTO> GetByIDAsync(int ID)
        {
            var author = await _repository.GetByIDAsync(ID);
            return _mapper.Map<AuthorDTO>(author);
        }

        public async Task CreateAsync(AuthorDTO dto)
        {
            var author = _mapper.Map<Author>(dto);
            await _repository.AddAsync(author);
        }

        public async Task UpdateAsync(int ID, AuthorDTO dto)
        {
            var existing = await _repository.GetByIDAsync(ID);
            if (existing == null) throw new Exception("Author not found");

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int ID)
        {
            var author = await _repository.GetByIDAsync(ID);
            if (author == null) throw new Exception("Author not found");

            await _repository.DeleteAsync(author);
        }
    }
}
