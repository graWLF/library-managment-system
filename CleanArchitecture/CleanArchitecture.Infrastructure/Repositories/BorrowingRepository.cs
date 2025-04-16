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
    public class BorrowingRepository : IBorrowingRepository
    {
        private readonly ApplicationDbContext _context;

        public BorrowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Borrowing>> GetAllAsync()
        {
            return await _context.borrowings.ToListAsync();
        }

        public async Task<Borrowing> GetByIDAsync(long ID)
        {
            return await _context.borrowings.FindAsync(ID);
        }

        public async Task AddAsync(Borrowing borrowing)
        {
            _context.borrowings.Add(borrowing);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Borrowing borrowing)
        {
            _context.borrowings.Update(borrowing);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Borrowing borrowing)
        {
            _context.borrowings.Remove(borrowing);
            await _context.SaveChangesAsync();
        }
    }
}
