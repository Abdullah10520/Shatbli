using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shatabli.Core.Domain.Enums;

namespace Shatabli.Core.Domain.Entities
{
    public class Design : BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; } = null!;

        public string OriginalImagePath { get; set; } = string.Empty;
        public string OriginalImageUrl { get; set; } = string.Empty;

        public string? GeneratedImagePath { get; set; }
        public string? GeneratedImageUrl { get; set; }

        public DesignType DesignType { get; set; }

        public string? ProductImagePath { get; set; }
        public string? ProductImageUrl { get; set; }
        public DesignStatus Status { get; set; } = DesignStatus.Pending;
        public string? ErrorMessage { get; set; }

        public DateTime? CompletedAt { get; set; }
    }
}
