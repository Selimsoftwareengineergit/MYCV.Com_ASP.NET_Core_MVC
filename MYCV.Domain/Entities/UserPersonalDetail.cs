using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MYCV.Domain.Enums;

namespace MYCV.Domain.Entities
{
    public class UserPersonalDetail : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required, MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string ProfessionalTitle { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        public Gender? Gender { get; set; }

        [Required, MaxLength(150), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Address { get; set; }

        [MaxLength(250)]
        public string? ProfilePictureUrl { get; set; }

        [MaxLength(250)]
        public string? LinkedIn { get; set; }

        [MaxLength(250)]
        public string? GitHub { get; set; }

        [MaxLength(250)]
        public string? Portfolio { get; set; }

        [MaxLength(250)]
        public string? Website { get; set; }

        [MaxLength(250)]
        public string? LinkedInHeadline { get; set; }
    }
}