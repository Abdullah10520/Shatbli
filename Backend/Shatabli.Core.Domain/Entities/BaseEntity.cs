using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Domain.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DelatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        public string? DeletedBy { get; set; }
    }
}
