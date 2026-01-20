using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Domain.Entities
{
    public class WorkExperience : BaseEntity
    {
        [Required]
        public int UserCvId { get; set; }
        public UserCv UserCv { get; set; } = null!;

        [MaxLength(200)]
        public string Company { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Position { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [MaxLength(1000)]
        public string Responsibilities { get; set; } = string.Empty;
    }
}