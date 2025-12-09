namespace Shatbli.Data.Models
{   
    public class AIProcessingLog : BaseEntity
    {
        public int DesignId { get; set; }
        public Design Design { get; set; } = null!;
        
        // Request details
        public string Prompt { get; set; } = string.Empty;
        public string RequestPayload { get; set; } = string.Empty;
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        
        // Response details
        public string? ResponsePayload { get; set; }
        public DateTime? RespondedAt { get; set; }
        public int ResponseTimeMs { get; set; }
        
        // Error handling
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ErrorStackTrace { get; set; }
        
        // API usage tracking
        public string ApiEndpoint { get; set; } = string.Empty;
        public string ApiVersion { get; set; } = string.Empty;
        public decimal? ApiCost { get; set; }
    }
}