using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Core.Entities;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public class BorrowerRepository : IBorrowerRepository
    {
        private readonly ApplicationDbContext _context;

        public BorrowerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Borrower>> GetAllAsync()
        {
            return await _context.Borrowers.ToListAsync();
        }

        public async Task<Borrower> GetByIDAsync(int ID)
        {
            return await _context.Borrowers.FindAsync(ID);
        }

        public async Task AddAsync(Borrower borrower)
        {
            _context.Borrowers.Add(borrower);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Borrower borrower)
        {
            _context.Borrowers.Update(borrower);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Borrower borrower)
        {
            _context.Borrowers.Remove(borrower);
            await _context.SaveChangesAsync();
        }
    }
}
