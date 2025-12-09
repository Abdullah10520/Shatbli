using Microsoft.AspNetCore.Http;
using Shatbli.Data.DTOs.DesignDtos;
using Shatbli.Data.Models;

namespace Shatbli.Service.Interfaces
{
    public interface IDesignService
    {
        Task<Design> CreateCeramicDesignAsync(int userId, CreateCeramicDesignDto dto);
        Task<Design> CreateWallPaintDesignAsync(int userId, CreateWallPaintDesignDto dto);
        Task<Design?> GetDesignByIdAsync(int designId, int userId);
        Task<List<Design>> GetUserDesignsAsync(int userId);
        Task<bool> ProcessDesignWithAIAsync(int designId);
        Task<string> SaveImageAsync(IFormFile file, string folder);
        Task<byte[]?> GetImageForDownloadAsync(int designId, int userId, bool isGenerated);
        Task MarkAsDownloadedAsync(int designId);
    }
}