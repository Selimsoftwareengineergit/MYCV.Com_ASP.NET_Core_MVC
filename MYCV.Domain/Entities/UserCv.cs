using MYCV.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYCV.Domain.Entities
{
    public class UserCv : BaseEntity
    {
        public string FullName { get; set; } = null!;
        public string ProfessionalTitle { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
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

        // Relationships to CV-related entities
        public virtual ICollection<UserExperiences> UserExperiences { get; set; } = new List<UserExperiences>();
        public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
        public virtual ICollection<Language> Languages { get; set; } = new List<Language>();

        // Foreign key to User (one-to-one)
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}