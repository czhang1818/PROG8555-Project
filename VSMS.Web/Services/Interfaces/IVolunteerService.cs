using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSMS.Web.Models;

namespace VSMS.Web.Services.Interfaces
{
    public interface IVolunteerService
    {
        Task<IEnumerable<Volunteer>> GetAllVolunteersAsync();
        Task<Volunteer?> GetVolunteerByIdAsync(Guid id);
        Task AddVolunteerAsync(Volunteer volunteer);
        Task UpdateVolunteerAsync(Volunteer volunteer);
        Task DeleteVolunteerAsync(Guid id);
        Task AddOrUpdateVolunteerSkillsAsync(Guid volunteerId, List<Guid> skillIds);
        bool VolunteerExists(Guid id);
    }
}
