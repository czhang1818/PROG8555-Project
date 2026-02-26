using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSMS.Web.Models;

namespace VSMS.Web.Services.Interfaces
{
    public interface IOpportunityService
    {
    
        /// retrieves all available opportunities.
  
        Task<IEnumerable<Opportunity>> GetAllOpportunitiesAsync();
        
  
        /// retrieves a specific opportunity by its unique identifier.
        Task<Opportunity?> GetOpportunityByIdAsync(Guid id);
       
        /// adds a new opportunity to the system.
        Task AddOpportunityAsync(Opportunity opportunity);

        /// updates the details of an existing opportunity.
        Task UpdateOpportunityAsync(Opportunity opportunity);

        /// deletes an opportunity from the system based on its unique identifier.
                Task DeleteOpportunityAsync(Guid id);

        
        Task AddOrUpdateOpportunitySkillsAsync(Guid opportunityId, List<Guid> skillIds);
        bool OpportunityExists(Guid id);
    }
}
