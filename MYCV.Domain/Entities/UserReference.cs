using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MYCV.Domain.Entities
{
    public class UserReference : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? Position { get; set; }  

        [MaxLength(150)]
        public string? Company { get; set; }  

        [Required, MaxLength(100)]
        public string Contact { get; set; } = string.Empty; 

        [MaxLength(500)]
        public string? Relation { get; set; } 

        public int Priority { get; set; } = 1; 
    }
}