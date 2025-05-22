using AutoMapper;
using CleanArchitecture.Core.DTOs.BookCopy;
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
    public class BookCopyService : IBookCopyService
    {
        private readonly IBookCopyRepository _repository;
        private readonly IMapper _mapper;

        public BookCopyService(IBookCopyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookCopyDTO>> GetAllAsync()
        {
            var bookCopies = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<BookCopyDTO>>(bookCopies);
        }
        public async Task DeleteByIsbnAsync(long Isbn)
        {
            var bookCopies = await _repository.GetAllByIsbnAsync(Isbn);
            if (bookCopies == null || !bookCopies.Any()) throw new Exception("BookCopy not found");
            foreach (var bookCopy in bookCopies)
            {
                await _repository.DeleteAsync(bookCopy);
            }

        }

        public async Task<BookCopyDTO> GetByIdAsync(long Id)
        {
            var bookCopy = await _repository.GetByIdAsync(Id);
            return _mapper.Map<BookCopyDTO>(bookCopy);
        }

        public async Task CreateAsync(BookCopyDTO dto)
        {
            var bookCopy = _mapper.Map<BookCopy>(dto);
            await _repository.AddAsync(bookCopy);
        }

        public async Task UpdateAsync(long Id, BookCopyDTO dto)
        {
            var existing = await _repository.GetByIdAsync(Id);
            if (existing == null) throw new Exception("bookCopy not found");

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(long Id)
        {
            var bookCopy = await _repository.GetByIdAsync(Id);
            if (bookCopy == null) throw new Exception("BookCopy not found");

            await _repository.DeleteAsync(bookCopy);
        }
    }
}
