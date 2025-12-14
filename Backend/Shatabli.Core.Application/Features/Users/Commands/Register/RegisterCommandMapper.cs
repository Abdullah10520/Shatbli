using AutoMapper;
using Shatabli.Core.Domain.Entities;

namespace Shatabli.Core.Application.Features.Users.Commands.Register
{
    public class RegisterCommandMapper : Profile
    {
        public RegisterCommandMapper()
        {
            CreateMap<RegisterCommand, User>()
             .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
             .ForMember(dest => dest.Role, opt => opt.Ignore())
             .ForMember(dest => dest.IsActive, opt => opt.Ignore())
             .ForMember(dest => dest.IsEmailVerified, opt => opt.Ignore())
             .ForMember(dest => dest.Designs, opt => opt.Ignore())
             .ForMember(dest => dest.LastLoginAt, opt => opt.Ignore())
             .ForMember(dest => dest.Id, opt => opt.Ignore())
             .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
             .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}
