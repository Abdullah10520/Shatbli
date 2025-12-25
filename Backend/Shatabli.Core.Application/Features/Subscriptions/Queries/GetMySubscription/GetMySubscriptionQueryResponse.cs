namespace Shatabli.Core.Application.Features.Subscriptions.Queries.GetMySubscription
{
    public class GetMySubscriptionQueryResponse
    {
        public string Id { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public string PlanType { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public int ImagesGeneratedToday { get; set; }
        public int ImagesGeneratedThisMonth { get; set; }
        public int MaxImagesPerDay { get; set; }
        public int MaxImagesPerMonth { get; set; }
    }
}