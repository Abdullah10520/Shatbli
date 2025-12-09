using AutoMapper;
using Shatbli.Data.DTOs.UserDtos;
using Shatbli.Data.Models;

namespace Shatbli.Core.Mapping.UserMapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserProfileDto>()
                .ForMember(dest => dest.TotalDesigns, opt => opt.MapFrom(src => src.Designs.Count));
        }
    }
}