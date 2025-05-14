using CleanArchitecture.Core.Entities;
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
        public async Task<BookCopy> GetByCompositeKeyAsync(long itemNo, long isbn)
        {
            return await _context.BookCopies.FirstOrDefaultAsync(b =>
                b.itemno == itemNo &&
                b.isbn == isbn);
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
