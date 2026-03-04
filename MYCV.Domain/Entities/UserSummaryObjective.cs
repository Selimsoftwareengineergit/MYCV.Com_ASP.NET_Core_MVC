using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MYCV.Domain.Entities
{
    public class UserSummaryObjective : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required, MaxLength(1500)]
        public string Summary { get; set; } = string.Empty;

        [Required, MaxLength(1000)]
        public string Objective { get; set; } = string.Empty;
        public int Priority { get; set; } = 1;
    }
}