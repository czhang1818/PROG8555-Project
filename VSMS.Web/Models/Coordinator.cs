using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VSMS.Web.Models
{
    public class Coordinator : User
    {
        [Required]
        public Guid OrganizationId { get; set; } // Foreign Key

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        // Navigation Property
        [ForeignKey("OrganizationId")]
        public Organization? Organization { get; set; }
    }
}
