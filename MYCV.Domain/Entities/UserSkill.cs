using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Domain.Entities
{
    public class UserSkill : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required, MaxLength(100)]
        public string SkillName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Level { get; set; } = string.Empty; // e.g., Beginner, Intermediate, Expert

        [MaxLength(250)]
        public string Remarks { get; set; } = string.Empty;

        // Optional: Skill category or type (e.g., Technical, Soft, Language)
        [MaxLength(50)]
        public string? SkillType { get; set; }

        // Optional: Years of experience with this skill
        public double? YearsOfExperience { get; set; }

        // Optional: Certification or proof for this skill
        [MaxLength(250)]
        public string? CertificateFile { get; set; }

        // Optional: Priority or importance level in CV (e.g., 1-5)
        public int Priority { get; set; } = 1;

        // Optional: Proficiency percentage (0-100) for detailed scoring
        public int? ProficiencyPercentage { get; set; }
    }
}