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
    public class LibrarianRepository : ILibrarianRepository
    {
        private readonly ApplicationDbContext _context;

        public LibrarianRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Librarian>> GetAllAsync()
        {
            return await _context.librarians.ToListAsync();
        }

        public async Task<Librarian> GetByIDAsync(int librarianID)
        {
            return await _context.librarians.FindAsync(librarianID);
        }

        public async Task AddAsync(Librarian librarian)
        {
            _context.librarians.Add(librarian);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Librarian librarian)
        {
            _context.librarians.Update(librarian);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Librarian librarian)
        {
            _context.librarians.Remove(librarian);
            await _context.SaveChangesAsync();
        }
    }
}
