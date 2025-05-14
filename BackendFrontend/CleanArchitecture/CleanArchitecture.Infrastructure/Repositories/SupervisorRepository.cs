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
    public class SupervisorRepository : ISupervisorRepository
    {
        private readonly ApplicationDbContext _context;

        public SupervisorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Supervisor>> GetAllAsync()
        {
            return await _context.supervisors.ToListAsync();
        }

        public async Task<Supervisor> GetByIDAsync(int SupervisorID)
        {
            return await _context.supervisors.FindAsync(SupervisorID);
        }

        public async Task AddAsync(Supervisor supervisor)
        {
            _context.supervisors.Add(supervisor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Supervisor supervisor)
        {
            _context.supervisors.Update(supervisor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Supervisor supervisor)
        {
            _context.supervisors.Remove(supervisor);
            await _context.SaveChangesAsync();
        }
    }
}
