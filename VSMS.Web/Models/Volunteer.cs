using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VSMS.Web.Models
{
    public class Volunteer : User
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        public float TotalHours { get; set; } = 0;

        // Navigation Properties
        public ICollection<Application>? Applications { get; set; }
        public ICollection<VolunteerSkill>? VolunteerSkills { get; set; }
    }
}
