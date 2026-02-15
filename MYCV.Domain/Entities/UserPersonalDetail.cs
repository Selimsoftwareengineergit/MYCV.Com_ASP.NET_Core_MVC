using MYCV.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYCV.Domain.Entities
{
    public class UserPersonalDetail : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string ProfessionalTitle { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? Address { get; set; }
        public string? ProfilePictureUrl { get; set; }

        public string? Summary { get; set; }
        public string? Objective { get; set; }

        public string? LinkedIn { get; set; }
        public string? GitHub { get; set; }
        public string? Portfolio { get; set; }
        public string? Website { get; set; }
        public string? LinkedInHeadline { get; set; }
    }
}