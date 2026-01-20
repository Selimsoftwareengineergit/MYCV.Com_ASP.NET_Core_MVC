using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Domain.Entities
{
    public class Project : BaseEntity
    {
        [Required]
        public int UserCvId { get; set; }
        public UserCv UserCv { get; set; } = null!;

        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(300)]
        public string Link { get; set; } = string.Empty;
    }
}