using AutoMapper;
using CleanArchitecture.Core.DTOs.Librarian;
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
    public class LibrarianService : ILibrarianService
    {
        private readonly ILibrarianRepository _repository;
        private readonly IMapper _mapper;

        public LibrarianService(ILibrarianRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LibrarianDTO>> GetAllAsync()
        {
            var librarians = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<LibrarianDTO>>(librarians);
        }

        public async Task<LibrarianDTO> GetByIDAsync(int ID)
        {
            var librarian = await _repository.GetByIDAsync(ID);
            return _mapper.Map<LibrarianDTO>(librarian);
        }

        public async Task CreateAsync(LibrarianDTO dto)
        {
            var librarian = _mapper.Map<Librarian>(dto);
            await _repository.AddAsync(librarian);
        }

        public async Task UpdateAsync(int ID, LibrarianDTO dto)
        {
            var existing = await _repository.GetByIDAsync(ID);
            if (existing == null) throw new Exception("Librarian not found");

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int ID)
        {
            var librarian = await _repository.GetByIDAsync(ID);
            if (librarian == null) throw new Exception("Librarian not found");

            await _repository.DeleteAsync(librarian);
        }
    }
}
