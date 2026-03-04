using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MYCV.Domain.Enums;

namespace MYCV.Domain.Entities
{
    public class UserLanguage : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Language { get; set; } = string.Empty;

        [MaxLength(20)]
        public LanguageProficiency? Proficiency { get; set; }

        public int Priority { get; set; } = 1;
    }
}