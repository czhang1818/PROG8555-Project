using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VSMS.Web.Data;
using VSMS.Web.Models;
using VSMS.Web.Services.Interfaces;

namespace VSMS.Web.Services
{
    public class CoordinatorService : ICoordinatorService
    {
        private readonly AppDbContext _context;

        public CoordinatorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Coordinator>> GetAllCoordinatorsAsync()
        {
            return await _context.Coordinators.Include(c => c.Organization).ToListAsync();
        }

        public async Task<Coordinator?> GetCoordinatorByIdAsync(Guid id)
        {
            return await _context.Coordinators
                .Include(c => c.Organization)
                .FirstOrDefaultAsync(m => m.UserId == id);
        }

        public async Task AddCoordinatorAsync(Coordinator coordinator)
        {
            _context.Add(coordinator);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCoordinatorAsync(Coordinator coordinator)
        {
            _context.Update(coordinator);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCoordinatorAsync(Guid id)
        {
            var coordinator = await _context.Coordinators.FindAsync(id);
            if (coordinator != null)
            {
                _context.Coordinators.Remove(coordinator);
                await _context.SaveChangesAsync();
            }
        }

        public bool CoordinatorExists(Guid id)
        {
            return _context.Coordinators.Any(e => e.UserId == id);
        }
    }
}
