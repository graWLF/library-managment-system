﻿using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public class BookCopyRepository : IBookCopyRepository
    {
        private readonly ApplicationDbContext _context;

        public BookCopyRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BookCopy>> GetAllAsync()
        {
            return await _context.BookCopies.ToListAsync();
        }
        public async Task<BookCopy> GetByIdAsync(long Id)
        {
            return await _context.BookCopies.FindAsync(Id);
        }
        public async Task<IEnumerable<BookCopy>> GetAllByIsbnAsync(long Isbn)
        {
            return await _context.BookCopies.Where(b => b.isbn == Isbn).ToListAsync();
        }


        public async Task AddAsync(BookCopy bookCopy)
        {
            _context.BookCopies.Add(bookCopy);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(BookCopy bookCopy)
        {
            _context.BookCopies.Update(bookCopy);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(BookCopy bookCopy)
        {
            _context.BookCopies.Remove(bookCopy);
            await _context.SaveChangesAsync();
        }
    }
}
