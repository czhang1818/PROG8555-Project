using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSMS.Web.Models;

namespace VSMS.Web.Services.Interfaces
{
    public interface IOpportunityService
    {
        Task<IEnumerable<Opportunity>> GetAllOpportunitiesAsync();
        Task<Opportunity?> GetOpportunityByIdAsync(Guid id);
        Task AddOpportunityAsync(Opportunity opportunity);
        Task UpdateOpportunityAsync(Opportunity opportunity);
        Task DeleteOpportunityAsync(Guid id);
        Task AddOrUpdateOpportunitySkillsAsync(Guid opportunityId, List<Guid> skillIds);
        bool OpportunityExists(Guid id);
    }
}
