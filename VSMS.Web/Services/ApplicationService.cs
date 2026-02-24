using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VSMS.Web.Data;
using VSMS.Web.Models;
using VSMS.Web.Services.Interfaces;

namespace VSMS.Web.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly AppDbContext _context;

        public ApplicationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Application>> GetAllApplicationsAsync()
        {
            return await _context.Applications
                .Include(a => a.Opportunity)
                .Include(a => a.Volunteer)
                .ToListAsync();
        }

        public async Task<Application?> GetApplicationByIdAsync(Guid id)
        {
            return await _context.Applications
                .Include(a => a.Opportunity)
                .Include(a => a.Volunteer)
                .FirstOrDefaultAsync(m => m.AppId == id);
        }

        public async Task AddApplicationAsync(Application application)
        {
            _context.Add(application);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateApplicationAsync(Application application)
        {
            _context.Update(application);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteApplicationAsync(Guid id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application != null)
            {
                _context.Applications.Remove(application);
                await _context.SaveChangesAsync();
            }
        }

        public bool ApplicationExists(Guid id)
        {
            return _context.Applications.Any(e => e.AppId == id);
        }
    }
}
