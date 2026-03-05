using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MYCV.Domain.Enums;

namespace MYCV.Domain.Entities
{
    public class UserEducation : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        // Education Level (SSC, HSC, Bachelor, etc.)
        [Required]
        public EducationLevel EducationLevel { get; set; } = EducationLevel.Others;

        // Degree / Exam Name
        [Required, MaxLength(150)]
        public string ExamName { get; set; } = string.Empty;

        // School / College / University
        [MaxLength(150)]
        public string? InstituteName { get; set; }

        // Board or University
        [MaxLength(100)]
        public string? BoardOrUniversity { get; set; }

        // Group / Major
        [MaxLength(100)]
        public string? GroupOrMajor { get; set; }

        [MaxLength(50)]
        public string? Result { get; set; }

        [MaxLength(50)]
        public string? Duration { get; set; }

        [Range(1900, 2100)]
        public int? PassingYear { get; set; }

        // Currently studying (checkbox)
        public bool IsCurrentlyStudying { get; set; } = false;

        // Extra note
        [MaxLength(250)]
        public string? Remarks { get; set; }
    }
}