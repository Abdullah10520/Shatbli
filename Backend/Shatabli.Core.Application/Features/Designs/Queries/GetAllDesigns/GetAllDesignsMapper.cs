using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Shatabli.Core.Domain.Entities;

namespace Shatabli.Core.Application.Features.Designs.Queries.GetAllDesigns
{
    public class GetAllDesignsMapper : Profile
    {
        public GetAllDesignsMapper() 
        {
            CreateProjection<Design, GetAllDesignDTO>()
                .ForMember(dest => dest.designId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.generatedImageUrl, opt => opt.MapFrom(src => src.GeneratedImageUrl))
                .ForMember(dest => dest.completedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }    
    }
}
