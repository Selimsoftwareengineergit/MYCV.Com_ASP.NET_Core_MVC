using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYCV.Domain.Entities
{
    public class UserEducation : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required, MaxLength(50)]
        public string EducationLevel { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string ExamName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string BoardOrUniversity { get; set; } = string.Empty;

        [MaxLength(50)]
        public string GroupOrMajor { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Result { get; set; } = string.Empty;

        public int PassingYear { get; set; }

        [MaxLength(250)]
        public string CertificateFile { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Remarks { get; set; } = string.Empty;
    }
}