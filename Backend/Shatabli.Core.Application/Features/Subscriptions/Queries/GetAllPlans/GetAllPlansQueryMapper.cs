using AutoMapper;
using Shatabli.Core.Domain.Entities;


namespace Shatabli.Core.Application.Features.Subscriptions.Queries.GetAllPlans
{
    public class GetAllPlansQueryMapper : Profile
    {
        public GetAllPlansQueryMapper()
        {
            CreateMap<SubscriptionPlan, GetAllPlansQueryResponse>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
        }
    }
}
