using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.DTOs.BookCopy;
using CleanArchitecture.Core.Entities;
using AutoMapper;

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
        public async Task<BookCopyDTO> GetByCompositeKeyAsync(long itemNo, long isbn)
        {
            var entity = await _repository.GetByCompositeKeyAsync(itemNo, isbn);
            return _mapper.Map<BookCopyDTO>(entity);
        }

        public async Task CreateAsync(BookCopyDTO dto)
        {
            var bookCopy = _mapper.Map<BookCopy>(dto);
            await _repository.AddAsync(bookCopy);
        }
        public async Task UpdateAsync(long itemNo, long copyNo, BookCopyDTO dto)
        {
            var existing = await _repository.GetByCompositeKeyAsync(itemNo, dto.isbn);
            if (existing == null) throw new Exception("BookCopy not found");
            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
        }
        public async Task DeleteAsync(long itemNo, long copyNo)
        {
            var existing = await _repository.GetByCompositeKeyAsync(itemNo, copyNo);
            if (existing == null) throw new Exception("BookCopy not found");
            await _repository.DeleteAsync(existing);
        }
    }
}
