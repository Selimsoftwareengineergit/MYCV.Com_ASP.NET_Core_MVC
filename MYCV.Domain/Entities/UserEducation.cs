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

        [Required]
        public EducationLevel EducationLevel { get; set; } = EducationLevel.Others;

        [Required, MaxLength(100)]
        public string ExamName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? BoardOrUniversity { get; set; }

        [MaxLength(50)]
        public string? GroupOrMajor { get; set; }

        [MaxLength(50)]
        public string? Result { get; set; }

        [Range(1900, 2100)]
        public int PassingYear { get; set; }

        [MaxLength(250)]
        public string? CertificateFile { get; set; }

        [MaxLength(250)]
        public string? Remarks { get; set; }
    }
}