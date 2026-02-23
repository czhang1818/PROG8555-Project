using System;
using System.ComponentModel.DataAnnotations;

namespace VSMS.Web.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid();

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty; // e.g., "Volunteer", "Coordinator"

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
