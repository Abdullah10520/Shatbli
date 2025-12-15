using Shatabli.Core.Domain.Enums;

namespace Shatabli.Core.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public ProductCategory Category { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
