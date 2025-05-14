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
    public class BranchRepository : IBranchRepository
    {
        private readonly ApplicationDbContext _context;

        public BranchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Branch>> GetAllAsync()
        {
            return await _context.branches.ToListAsync();
        }

        public async Task<Branch> GetByIDAsync(int branchID)
        {
            return await _context.branches.FindAsync(branchID);
        }

        public async Task AddAsync(Branch branch)
        {
            _context.branches.Add(branch);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Branch branch)
        {
            _context.branches.Update(branch);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Branch branch)
        {
            _context.branches.Remove(branch);
            await _context.SaveChangesAsync();
        }
    }
}
