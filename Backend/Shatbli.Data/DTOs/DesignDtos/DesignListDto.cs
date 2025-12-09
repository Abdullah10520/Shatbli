using Shatbli.Data.Models.Enums;

namespace Shatbli.Data.DTOs.DesignDtos
{
    public class DesignListDto
    {
        public int Id { get; set; }
        public DesignType DesignType { get; set; }
        public string OriginalImageUrl { get; set; } = string.Empty;
        public string? GeneratedImageUrl { get; set; }
        public DesignStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDownloaded { get; set; }
    }
}