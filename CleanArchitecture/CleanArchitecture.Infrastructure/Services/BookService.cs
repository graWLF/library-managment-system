using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.DTOs;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.DTOs.Book;
using AutoMapper;
using System;
using CleanArchitecture.Core.Entities;

namespace CleanArchitecture.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            var books = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> GetByISBNAsync(long isbn)
        {
            var book = await _repository.GetByISBNAsync(isbn);
            return _mapper.Map<BookDto>(book);
        }

        public async Task CreateAsync(BookDto dto)
        {
            var book = _mapper.Map<Book>(dto);
            await _repository.AddAsync(book);
        }

        public async Task UpdateAsync(long isbn, BookDto dto)
        {
            var existing = await _repository.GetByISBNAsync(isbn);
            if (existing == null) throw new Exception("Book not found");

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(long isbn)
        {
            var book = await _repository.GetByISBNAsync(isbn);
            if (book == null) throw new Exception("Book not found");

            await _repository.DeleteAsync(book);
        }
    }

}

