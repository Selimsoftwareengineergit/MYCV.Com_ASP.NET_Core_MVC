using MYCV.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MYCV.Domain.Entities
{
    public class UserSubscription : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required]
        public SubscriptionPlan Plan { get; set; } = SubscriptionPlan.Monthly;

        [Required]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [MaxLength(100)]
        public string? PaymentTransactionId { get; set; } 

        [MaxLength(50)]
        public string? PaymentMethod { get; set; } 

        [MaxLength(250)]
        public string? Remarks { get; set; }

        public bool IsExpired => DateTime.UtcNow > EndDate;
    }
}