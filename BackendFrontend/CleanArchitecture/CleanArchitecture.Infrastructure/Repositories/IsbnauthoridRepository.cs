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
    public class IsbnauthoridRepository : IIsbnauthoridRepository
    {
        private readonly ApplicationDbContext _context;

        public IsbnauthoridRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Isbnauthorid>> GetAllAsync()
        {
            return await _context.isbnauthorids.ToListAsync();
        }

        public async Task<Isbnauthorid> GetByCompositeKeyAsync(long Id, long authorid)
        {
            return await _context.isbnauthorids.FirstOrDefaultAsync(b =>
                b.Id == Id &&
                b.authorid == authorid
                );
        }


        public async Task AddAsync(Isbnauthorid isbnauthorid)
        {
            _context.isbnauthorids.Add(isbnauthorid);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Isbnauthorid isbnauthorid)
        {
            _context.isbnauthorids.Update(isbnauthorid);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Isbnauthorid isbnauthorid)
        {
            _context.isbnauthorids.Remove(isbnauthorid);
            await _context.SaveChangesAsync();
        }
    }
}
