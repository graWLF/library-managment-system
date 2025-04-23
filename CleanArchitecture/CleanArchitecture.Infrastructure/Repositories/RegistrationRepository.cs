using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly ApplicationDbContext _context;

        public RegistrationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Registration>> GetAllAsync()
        {
            return await _context.registrations.ToListAsync();
        }

        public async Task<Registration> GetByIDAsync(int id)
        {
            return await _context.registrations.FindAsync(id);
        }

        public async Task AddAsync(Registration registration)
        {
            _context.registrations.Add(registration);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Registration registration)
        {
            _context.registrations.Update(registration);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Registration registration)
        {
            _context.registrations.Remove(registration);
            await _context.SaveChangesAsync();
        }
    }
}
