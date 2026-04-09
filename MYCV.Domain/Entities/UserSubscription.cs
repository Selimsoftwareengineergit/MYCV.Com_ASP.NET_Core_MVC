using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MYCV.Domain.Enums;

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
        public PaymentMethod PaymentMethod { get; set; }

        [MaxLength(100)]
        public string? PaymentTransactionId { get; set; }

        [MaxLength(250)]
        public string? Remarks { get; set; }

        [NotMapped]
        public bool IsExpired => DateTime.UtcNow > EndDate;

        [NotMapped]
        public decimal Amount => GetAmountByPlan(Plan);

        public void CalculateEndDate()
        {
            EndDate = Plan switch
            {
                SubscriptionPlan.Weekly => StartDate.AddDays(7),
                SubscriptionPlan.Monthly => StartDate.AddMonths(1),
                SubscriptionPlan.Quarterly => StartDate.AddMonths(3),
                SubscriptionPlan.HalfYearly => StartDate.AddMonths(6),
                SubscriptionPlan.Yearly => StartDate.AddYears(1),
                _ => StartDate.AddMonths(1)
            };
        }

        public static decimal GetAmountByPlan(SubscriptionPlan plan)
        {
            return plan switch
            {
                SubscriptionPlan.Weekly => 50,
                SubscriptionPlan.Monthly => 200,
                SubscriptionPlan.Quarterly => 500,
                SubscriptionPlan.HalfYearly => 900,
                SubscriptionPlan.Yearly => 1700,
                _ => 200
            };
        }
    }
}