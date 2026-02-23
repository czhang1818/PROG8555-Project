using System.ComponentModel.DataAnnotations;
using VSMS.Web.Models;

namespace VSMS.Web.ViewModels
{
    public class VolunteerSkillViewModel
    {
        public Volunteer Volunteer { get; set; } = new Volunteer();

        [Display(Name = "Volunteer Skills")]
        public List<Guid> SelectedSkillIds { get; set; } = new List<Guid>();

        // For displaying the checkboxes
        public List<Skill> AvailableSkills { get; set; } = new List<Skill>();
    }
}
