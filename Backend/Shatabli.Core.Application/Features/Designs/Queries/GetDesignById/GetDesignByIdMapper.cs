using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Shatabli.Core.Domain.Entities;

namespace Shatabli.Core.Application.Features.Designs.Queries.GetDesignById
{
    public class GetDesignByIdMapper : Profile
    {
        public GetDesignByIdMapper() 
        {
            //CreateProjection<Design, GetDesignByIdDTO>();
            CreateProjection<Design, GetDesignByIdDTO>()
            .ForMember(dest => dest.designId, opt => opt.MapFrom(src => src.Id))

            .ForMember(dest => dest.generatedImageUrl, opt => opt.MapFrom(src => src.GeneratedImageUrl))

            .ForMember(dest => dest.completedAt, opt => opt.MapFrom(src => src.CompletedAt ?? src.CreatedAt));
        }
    }
}
