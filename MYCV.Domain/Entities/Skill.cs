using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Domain.Entities
{
    public class Skill : BaseEntity
    {
        [Required]
        public int UserCvId { get; set; }
        public UserCv UserCv { get; set; } = null!;

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Level { get; set; } = string.Empty;
    }
}