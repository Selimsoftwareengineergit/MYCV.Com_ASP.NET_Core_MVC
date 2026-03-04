using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MYCV.Domain.Entities
{
    public class UserDownloadHistory : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required, MaxLength(50)]
        public string FileType { get; set; } = "PDF"; 

        [Required]
        public DateTime DownloadedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(250)]
        public string? Remarks { get; set; } 
    }
}