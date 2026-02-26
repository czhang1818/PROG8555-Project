using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSMS.Web.Models;

namespace VSMS.Web.Services.Interfaces
{
    public interface ISkillService
    {
     
        /// retrieves all available skills.
     
    
        Task<IEnumerable<Skill>> GetAllSkillsAsync();

       /// retrieves a specific skill by its unique identifier.


        Task<Skill?> GetSkillByIdAsync(Guid id);


  
        /// adds a new skill to the system.
  
          Task AddSkillAsync(Skill skill);


        /// updates the details of an existing skill.


        Task UpdateSkillAsync(Skill skill);

        Task DeleteSkillAsync(Guid id);

        bool SkillExists(Guid id);
    }
}
