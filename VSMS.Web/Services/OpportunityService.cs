using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VSMS.Web.Data;
using VSMS.Web.Models;
using VSMS.Web.Services.Interfaces;

namespace VSMS.Web.Services
{
    public class OpportunityService : IOpportunityService
    {
        private readonly AppDbContext _context;

        public OpportunityService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Opportunity>> GetAllOpportunitiesAsync()
        {
            return await _context.Opportunities.Include(o => o.Organization).ToListAsync();
        }

        public async Task<Opportunity?> GetOpportunityByIdAsync(Guid id)
        {
            return await _context.Opportunities
                .Include(o => o.Organization)
                .Include(o => o.OpportunitySkills)
                .FirstOrDefaultAsync(m => m.OpportunityId == id);
        }

        public async Task AddOpportunityAsync(Opportunity opportunity)
        {
            _context.Add(opportunity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOpportunityAsync(Opportunity opportunity)
        {
            _context.Update(opportunity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOpportunityAsync(Guid id)
        {
            var opportunity = await _context.Opportunities.FindAsync(id);
            if (opportunity != null)
            {
                _context.Opportunities.Remove(opportunity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddOrUpdateOpportunitySkillsAsync(Guid opportunityId, List<Guid> skillIds)
        {
            var existingSkills = await _context.OpportunitySkills
                .Where(os => os.OpportunityId == opportunityId)
                .ToListAsync();

            _context.OpportunitySkills.RemoveRange(existingSkills);

            if (skillIds != null && skillIds.Any())
            {
                var newSkills = skillIds.Select(skillId => new OpportunitySkill
                {
                    OpportunityId = opportunityId,
                    SkillId = skillId,
                    IsMandatory = false, // Defaulting as the UI doesn't capture this yet
                    MinimumLevel = "Beginner"
                });
                await _context.OpportunitySkills.AddRangeAsync(newSkills);
            }

            await _context.SaveChangesAsync();
        }

        public bool OpportunityExists(Guid id)
        {
            return _context.Opportunities.Any(e => e.OpportunityId == id);
        }
    }
}
