using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VSMS.Web.Models
{
    public class Organization
    {
        [Key]
        public Guid OrganizationId { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; } = string.Empty;

        [Url]
        public string? Website { get; set; }

        public bool IsVerified { get; set; } = false;

        // Navigation Properties
        public ICollection<Opportunity>? Opportunities { get; set; }
        public ICollection<Coordinator>? Coordinators { get; set; }
    }
}
