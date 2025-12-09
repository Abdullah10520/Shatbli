using Microsoft.AspNetCore.Http;

namespace Shatbli.Data.DTOs.DesignDtos
{
    public class CreateCeramicDesignDto
    {
        public IFormFile RoomImage { get; set; } = null!;

        // Option 1: Upload custom ceramic image
        public IFormFile? CeramicImage { get; set; }

        // Option 2: Select from existing products
        public int? CeramicProductId { get; set; }

        public string? CustomPrompt { get; set; }
    }
}