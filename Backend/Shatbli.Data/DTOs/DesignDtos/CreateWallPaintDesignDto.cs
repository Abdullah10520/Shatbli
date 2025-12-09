using Microsoft.AspNetCore.Http;

namespace Shatbli.Data.DTOs.DesignDtos
{
    public class CreateWallPaintDesignDto
    {
        public IFormFile RoomImage { get; set; } = null!;
        public string WallColor { get; set; } = string.Empty;
        public string? CustomPrompt { get; set; }
    }
}