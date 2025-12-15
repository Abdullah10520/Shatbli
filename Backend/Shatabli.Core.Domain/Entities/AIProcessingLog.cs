using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Domain.Entities
{
    public class AIProcessingLog : BaseEntity
    {
        public string DesignId { get; set; }
        public Design Design { get; set; } = null!;

        // Timing
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RespondedAt { get; set; }

        // Status
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
