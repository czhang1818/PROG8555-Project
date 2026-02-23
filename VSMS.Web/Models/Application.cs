using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VSMS.Web.Models
{
    public class Application
    {
        [Key]
        public Guid AppId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid VolunteerId { get; set; } // Foreign Key (Points to Volunteer's UserId)

        [Required]
        public Guid OpportunityId { get; set; } // Foreign Key

        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending";

        // Navigation Properties
        [ForeignKey("VolunteerId")]
        public Volunteer? Volunteer { get; set; }

        [ForeignKey("OpportunityId")]
        public Opportunity? Opportunity { get; set; }
    }
}
