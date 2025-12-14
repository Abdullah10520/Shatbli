using Shatabli.Core.Domain.Entities;

namespace Shatabli.Core.Application.Features.Users.Queries.GetProfile
{
    public class GetUserProfileQueryMapper : AutoMapper.Profile
    {
        public GetUserProfileQueryMapper()
        {
            CreateMap<User, GetUserProfileQueryResponse>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
        }
    }
}
