using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Domain.Entities
{
    public class UserProject : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required, MaxLength(200)]
        public string ProjectTitle { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Role { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Technologies { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsOngoing => EndDate == null;

        [MaxLength(250)]
        public string? ProjectLink { get; set; }

        [MaxLength(250)]
        public string? Remarks { get; set; }

        public int Priority { get; set; } = 1;
    }
}