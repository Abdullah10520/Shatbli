using MediatR;
using Microsoft.AspNetCore.Http;
using Shatbli.Core.Bases;
using Shatbli.Data.DTOs.DesignDtos;

namespace Shatbli.Core.Features.Designs.Commands.Models
{
    public class CreateCeramicDesignCommand : IRequest<Response<DesignResponseDto>>
    {
        public int UserId { get; set; }
        public IFormFile RoomImage { get; set; } = null!;
        public IFormFile? CeramicImage { get; set; }
        public int? CeramicProductId { get; set; }
        public string? CustomPrompt { get; set; }
    }
}