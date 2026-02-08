using MYCV.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Domain.Entities
{
    public class UserExperiences : BaseEntity
    {
        [Required]
        public int UserCvId { get; set; }  

        [ForeignKey(nameof(UserCvId))]
        public virtual UserCv UserCv { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Company { get; set; } = string.Empty; 

        [Required]
        [MaxLength(200)]
        public string Position { get; set; } = string.Empty; 

        [MaxLength(100)]
        public string Department { get; set; } = string.Empty; 

        [MaxLength(150)]
        public string Location { get; set; } = string.Empty; 

        [Required]
        public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime; 

        [Required]
        public DateTime StartDate { get; set; } 

        public DateTime? EndDate { get; set; } 

        [MaxLength(1000)]
        public string Responsibilities { get; set; } = string.Empty; 

        [MaxLength(250)]
        public string Remarks { get; set; } = string.Empty; 
    }
}