using MediatR;
using Microsoft.AspNetCore.Http;
using Shatbli.Core.Bases;
using Shatbli.Data.DTOs.DesignDtos;

namespace Shatbli.Core.Features.Designs.Commands.Models
{
    public class CreateWallPaintDesignCommand : IRequest<Response<DesignResponseDto>>
    {
        public int UserId { get; set; }
        public IFormFile RoomImage { get; set; } = null!;
        public string WallColor { get; set; } = string.Empty;
        public string? CustomPrompt { get; set; }
    }
}