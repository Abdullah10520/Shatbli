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
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Original image
        public string OriginalImagePath { get; set; } = string.Empty;
        public string OriginalImageUrl { get; set; } = string.Empty;
        public long OriginalImageSize { get; set; }

        // Generated image
        public string? GeneratedImagePath { get; set; }
        public string? GeneratedImageUrl { get; set; }
        public long? GeneratedImageSize { get; set; }

        // Design Type
        public DesignType DesignType { get; set; } = DesignType.CeramicFloor;

        // User selections for Ceramic Floor
        public int? SelectedCeramicProductId { get; set; }
        public Product? SelectedCeramicProduct { get; set; }
        public string? CeramicImagePath { get; set; }
        public string? CeramicImageUrl { get; set; }

        // User selections for Wall Paint
        public string? SelectedWallColor { get; set; }

        // AI Processing
        public string Prompt { get; set; } = string.Empty;
        public DesignStatus Status { get; set; } = DesignStatus.Pending;
        public string? ErrorMessage { get; set; }
        public int ProcessingTimeSeconds { get; set; }

        // Metadata
        public DateTime? CompletedAt { get; set; }
        public bool IsDownloaded { get; set; } = false;
        public DateTime? DownloadedAt { get; set; }
    }
}
