using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Domain.Entities
{
    public class Education : BaseEntity
    {
        public int UserCvId { get; set; }
        public UserCv UserCv { get; set; } = null!;

        public string Degree { get; set; } = null!;
        public string Institution { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
    }
}