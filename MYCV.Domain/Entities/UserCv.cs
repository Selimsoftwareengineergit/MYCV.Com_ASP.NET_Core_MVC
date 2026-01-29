using MYCV.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYCV.Domain.Entities
{
    public class UserCv : BaseEntity
    {
        // Step 1: Personal Information
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

        // Step 7: Summary & Objective
        public string? Summary { get; set; }
        public string? Objective { get; set; }

        // Step 8: Social & Online Presence
        public string? LinkedIn { get; set; }
        public string? GitHub { get; set; }
        public string? Portfolio { get; set; }
        public string? Website { get; set; }
        public string? LinkedInHeadline { get; set; }

        // Relationships
        public ICollection<UserEducation> UserEducations { get; set; } = new List<UserEducation>();
        public ICollection<WorkExperience> Experiences { get; set; } = new List<WorkExperience>();
        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Language> Languages { get; set; } = new List<Language>();

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}