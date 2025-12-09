using AutoMapper;
using Shatbli.Core.Features.Designs.Commands.Models;
using Shatbli.Data.DTOs.DesignDtos;
using Shatbli.Data.Models;

namespace Shatbli.Core.Mapping.DesignMapping
{
    public class DesignMappingProfile : Profile
    {
        public DesignMappingProfile()
        {
            // Commands to DTOs
            CreateMap<CreateCeramicDesignCommand, CreateCeramicDesignDto>()
                .ForMember(dest => dest.RoomImage, opt => opt.MapFrom(src => src.RoomImage))
                .ForMember(dest => dest.CeramicImage, opt => opt.MapFrom(src => src.CeramicImage))
                .ForMember(dest => dest.CeramicProductId, opt => opt.MapFrom(src => src.CeramicProductId))
                .ForMember(dest => dest.CustomPrompt, opt => opt.MapFrom(src => src.CustomPrompt));

            CreateMap<CreateWallPaintDesignCommand, CreateWallPaintDesignDto>()
                .ForMember(dest => dest.RoomImage, opt => opt.MapFrom(src => src.RoomImage))
                .ForMember(dest => dest.WallColor, opt => opt.MapFrom(src => src.WallColor))
                .ForMember(dest => dest.CustomPrompt, opt => opt.MapFrom(src => src.CustomPrompt));

            // Entity to DTOs
            CreateMap<Design, DesignResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DesignType, opt => opt.MapFrom(src => src.DesignType))
                .ForMember(dest => dest.OriginalImageUrl, opt => opt.MapFrom(src => src.OriginalImageUrl))
                .ForMember(dest => dest.GeneratedImageUrl, opt => opt.MapFrom(src => src.GeneratedImageUrl))
                .ForMember(dest => dest.CeramicImageUrl, opt => opt.MapFrom(src => src.CeramicImageUrl))
                .ForMember(dest => dest.SelectedWallColor, opt => opt.MapFrom(src => src.SelectedWallColor))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.ErrorMessage))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(src => src.CompletedAt))
                .ForMember(dest => dest.ProcessingTimeSeconds, opt => opt.MapFrom(src => src.ProcessingTimeSeconds));

            CreateMap<Design, DesignListDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DesignType, opt => opt.MapFrom(src => src.DesignType))
                .ForMember(dest => dest.OriginalImageUrl, opt => opt.MapFrom(src => src.OriginalImageUrl))
                .ForMember(dest => dest.GeneratedImageUrl, opt => opt.MapFrom(src => src.GeneratedImageUrl))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.IsDownloaded, opt => opt.MapFrom(src => src.IsDownloaded));
        }
    }
}