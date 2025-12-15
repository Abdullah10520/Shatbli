using System.ComponentModel.DataAnnotations;

namespace Shatabli.Core.Domain.Entities
{
    public class BaseEntity
    {
        //string
        public string Id { get; set; }
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
