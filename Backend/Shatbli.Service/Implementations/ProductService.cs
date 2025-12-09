using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shatbli.Data.DTOs.ProductDtos;
using Shatbli.Data.Models;
using Shatbli.Data.Models.Enums;
using Shatbli.Infrustructure.Abstracts;
using Shatbli.Service.Interfaces;

namespace Shatbli.Service.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _uploadsPath;
        private readonly string _baseUrl;

        public ProductService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "products");
            _baseUrl = configuration["AppSettings:BaseUrl"] ?? "https://localhost:7001";

            Directory.CreateDirectory(_uploadsPath);
        }

        public async Task<Product> CreateProductAsync(CreateProductDto dto)
        {
            var imagePath = await SaveProductImageAsync(dto.Image);
            var imageUrl = $"{_baseUrl}/uploads/products/{Path.GetFileName(imagePath)}";

            var product = new Product
            {
                Name = dto.Name,
                ImagePath = imagePath,
                ImageUrl = imageUrl,
                Vendor = dto.Vendor,
                Category = dto.Category,
                Texture = dto.Texture,
                Size = dto.Size,
                PricePerUnit = dto.PricePerUnit,
                ColorCode = dto.ColorCode,
                Brand = dto.Brand,
                IsActive = true
            };

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();

            return product;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null || product.IsDeleted)
                return false;

            // Delete physical file
            if (!string.IsNullOrEmpty(product.ImagePath) && File.Exists(product.ImagePath))
            {
                File.Delete(product.ImagePath);
            }

            // Soft delete
            product.IsDeleted = true;
            product.IsActive = false;
            _unitOfWork.Products.Update(product);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _unitOfWork.Products.FindAsync(p => p.Id == productId && !p.IsDeleted);
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(ProductCategory category, bool activeOnly = true)
        {
            var products = await _unitOfWork.Products.GetByCategoryAsync(category, activeOnly);
            return products.ToList();
        }

        public async Task<List<Product>> GetAllProductsAsync(bool activeOnly = true)
        {
            var products = activeOnly
                ? await _unitOfWork.Products.GetActiveProductsAsync()
                : await _unitOfWork.Products.FindAllAsync(p => !p.IsDeleted);

            return products.ToList();
        }

        public async Task<string> SaveProductImageAsync(IFormFile file)
        {
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(_uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }
    }
}