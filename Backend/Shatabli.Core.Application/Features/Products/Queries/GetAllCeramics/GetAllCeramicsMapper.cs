using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Shatabli.Core.Domain.Entities;

namespace Shatabli.Core.Application.Features.Products.Queries.GetAllCeramics
{
    public class GetAllCeramicsMapper : Profile
    {
        public GetAllCeramicsMapper() 
        {
            //CreateProjection<Product, CeramicDTO>();
            CreateProjection<Product, CeramicDTO>()
            .ForMember(dest => dest.productId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.productName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.productImageUrl, opt => opt.MapFrom(src => src.ImageUrl));
        }
    }
}
