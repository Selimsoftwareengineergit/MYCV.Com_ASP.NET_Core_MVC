using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYCV.Domain.Entities
{
    public class UserEducation : BaseEntity
    {
        [Required]
        public int UserCvId { get; set; }  

        [ForeignKey(nameof(UserCvId))]
        public virtual UserCv UserCv { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string EducationLevel { get; set; } = string.Empty;
        // e.g., Secondary, Higher Secondary, Graduate, Masters

        [Required]
        [MaxLength(100)]
        public string ExamName { get; set; } = string.Empty;
        // e.g., SSC, HSC, BSc, MSc

        [MaxLength(100)]
        public string BoardOrUniversity { get; set; } = string.Empty;

        [MaxLength(50)]
        public string GroupOrMajor { get; set; } = string.Empty;
        // Science, Arts, Commerce, Major for BSc/MSc

        [MaxLength(50)]
        public string Result { get; set; } = string.Empty;
        // CGPA, GPA, Percentage

        public int PassingYear { get; set; }

        [MaxLength(250)]
        public string CertificateFile { get; set; } = string.Empty;
        // Optional uploaded certificate path

        [MaxLength(250)]
        public string Remarks { get; set; } = string.Empty;
    }
}