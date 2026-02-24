using System.ComponentModel.DataAnnotations;
using VSMS.Web.Models;

namespace VSMS.Web.ViewModels
{
    public class OpportunitySkillViewModel
    {
        public Opportunity Opportunity { get; set; } = new Opportunity();

        [Display(Name = "Required Skills")]
        public List<Guid> SelectedSkillIds { get; set; } = new List<Guid>();

        // For displaying the checkboxes
        public List<Skill> AvailableSkills { get; set; } = new List<Skill>();
    }
}
