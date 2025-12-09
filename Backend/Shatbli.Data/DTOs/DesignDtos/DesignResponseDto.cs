using Shatbli.Data.Models.Enums;

namespace Shatbli.Data.DTOs.DesignDtos
{
    public class DesignResponseDto
    {
        public int Id { get; set; }
        public DesignType DesignType { get; set; }
        public string OriginalImageUrl { get; set; } = string.Empty;
        public string? GeneratedImageUrl { get; set; }
        public string? CeramicImageUrl { get; set; }
        public string? SelectedWallColor { get; set; }
        public DesignStatus Status { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int ProcessingTimeSeconds { get; set; }
    }
}