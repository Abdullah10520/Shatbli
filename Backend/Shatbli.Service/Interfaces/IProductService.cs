using Microsoft.AspNetCore.Http;
using Shatbli.Data.DTOs.ProductDtos;
using Shatbli.Data.Models;
using Shatbli.Data.Models.Enums;

namespace Shatbli.Service.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(CreateProductDto dto);
        Task<bool> DeleteProductAsync(int productId);
        Task<Product?> GetProductByIdAsync(int productId);
        Task<List<Product>> GetProductsByCategoryAsync(ProductCategory category, bool activeOnly = true);
        Task<List<Product>> GetAllProductsAsync(bool activeOnly = true);
        Task<string> SaveProductImageAsync(IFormFile file);
    }
}