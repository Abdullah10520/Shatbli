using AutoMapper;
using Shatabli.Core.Domain.Entities;


namespace Shatabli.Core.Application.Features.Subscriptions.Queries.GetMySubscription
{
    public class GetMySubscriptionQueryMapper : Profile
    {
        public GetMySubscriptionQueryMapper() 
        {
            CreateMap<UserSubscription, GetMySubscriptionQueryResponse>()
              .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.SubscriptionPlan.Name))
              .ForMember(dest => dest.PlanType, opt => opt.MapFrom(src => src.SubscriptionPlan.Type.ToString()))
              .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.SubscriptionPlan.Price))
              .ForMember(dest => dest.MaxImagesPerDay, opt => opt.MapFrom(src => src.SubscriptionPlan.MaxImagesPerDay))
              .ForMember(dest => dest.MaxImagesPerMonth, opt => opt.MapFrom(src => src.SubscriptionPlan.MaxImagesPerMonth));
        }
    }
}
