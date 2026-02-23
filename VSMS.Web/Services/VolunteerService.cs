using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VSMS.Web.Data;
using VSMS.Web.Models;
using VSMS.Web.Services.Interfaces;

namespace VSMS.Web.Services
{
    public class VolunteerService : IVolunteerService
    {
        private readonly AppDbContext _context;

        public VolunteerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Volunteer>> GetAllVolunteersAsync()
        {
            return await _context.Volunteers.ToListAsync();
        }

        public async Task<Volunteer?> GetVolunteerByIdAsync(Guid id)
        {
            return await _context.Volunteers.FirstOrDefaultAsync(m => m.UserId == id);
        }

        public async Task AddVolunteerAsync(Volunteer volunteer)
        {
            _context.Add(volunteer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVolunteerAsync(Volunteer volunteer)
        {
            _context.Update(volunteer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVolunteerAsync(Guid id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer != null)
            {
                _context.Volunteers.Remove(volunteer);
                await _context.SaveChangesAsync();
            }
        }

        public bool VolunteerExists(Guid id)
        {
            return _context.Volunteers.Any(e => e.UserId == id);
        }
    }
}
