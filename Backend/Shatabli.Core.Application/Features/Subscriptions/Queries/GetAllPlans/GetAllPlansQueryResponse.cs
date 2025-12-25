namespace Shatabli.Core.Application.Features.Subscriptions.Queries.GetAllPlans
{
    public class GetAllPlansQueryResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int MaxImagesPerMonth { get; set; }
        public int MaxImagesPerDay { get; set; }
        public bool HasWatermark { get; set; }
        public bool HasPriorityGeneration { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}