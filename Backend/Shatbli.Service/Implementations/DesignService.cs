using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shatbli.Data.DTOs.DesignDtos;
using Shatbli.Data.Models;
using Shatbli.Data.Models.Enums;
using Shatbli.Infrustructure.Abstracts;
using Shatbli.Service.Interfaces;
using System.Diagnostics;

namespace Shatbli.Service.Implementations
{
    public class DesignService : IDesignService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _uploadsPath;
        private readonly string _baseUrl;

        public DesignService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            _baseUrl = configuration["AppSettings:BaseUrl"] ?? "https://localhost:7001";

            // Create directories if they don't exist
            Directory.CreateDirectory(Path.Combine(_uploadsPath, "rooms"));
            Directory.CreateDirectory(Path.Combine(_uploadsPath, "ceramics"));
            Directory.CreateDirectory(Path.Combine(_uploadsPath, "generated"));
        }

        public async Task<Design> CreateCeramicDesignAsync(int userId, CreateCeramicDesignDto dto)
        {
            // Save room image
            var roomImagePath = await SaveImageAsync(dto.RoomImage, "rooms");
            var roomImageUrl = $"{_baseUrl}/uploads/rooms/{Path.GetFileName(roomImagePath)}";

            string? ceramicImagePath = null;
            string? ceramicImageUrl = null;

            // Option 1: User uploads custom ceramic image
            if (dto.CeramicImage != null)
            {
                ceramicImagePath = await SaveImageAsync(dto.CeramicImage, "ceramics");
                ceramicImageUrl = $"{_baseUrl}/uploads/ceramics/{Path.GetFileName(ceramicImagePath)}";
            }
            // Option 2: User selects from existing products
            else if (dto.CeramicProductId.HasValue)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(dto.CeramicProductId.Value);
                if (product != null)
                {
                    ceramicImagePath = product.ImagePath;
                    ceramicImageUrl = product.ImageUrl;
                }
            }

            var design = new Design
            {
                UserId = userId,
                DesignType = DesignType.CeramicFloor,
                OriginalImagePath = roomImagePath,
                OriginalImageUrl = roomImageUrl,
                OriginalImageSize = dto.RoomImage.Length,
                CeramicImagePath = ceramicImagePath,
                CeramicImageUrl = ceramicImageUrl,
                SelectedCeramicProductId = dto.CeramicProductId,
                Prompt = dto.CustomPrompt ?? "Apply ceramic floor texture to the room floor",
                Status = DesignStatus.Pending
            };

            await _unitOfWork.Designs.AddAsync(design);
            await _unitOfWork.CompleteAsync();

            return design;
        }

        public async Task<Design> CreateWallPaintDesignAsync(int userId, CreateWallPaintDesignDto dto)
        {
            // Save room image
            var roomImagePath = await SaveImageAsync(dto.RoomImage, "rooms");
            var roomImageUrl = $"{_baseUrl}/uploads/rooms/{Path.GetFileName(roomImagePath)}";

            var design = new Design
            {
                UserId = userId,
                DesignType = DesignType.WallPaint,
                OriginalImagePath = roomImagePath,
                OriginalImageUrl = roomImageUrl,
                OriginalImageSize = dto.RoomImage.Length,
                SelectedWallColor = dto.WallColor,
                Prompt = dto.CustomPrompt ?? $"Paint the walls with color {dto.WallColor}",
                Status = DesignStatus.Pending
            };

            await _unitOfWork.Designs.AddAsync(design);
            await _unitOfWork.CompleteAsync();

            return design;
        }

        public async Task<Design?> GetDesignByIdAsync(int designId, int userId)
        {
            return await _unitOfWork.Designs.FindAsync(
                d => d.Id == designId && d.UserId == userId && !d.IsDeleted,
                d => d.SelectedCeramicProduct!
            );
        }

        public async Task<List<Design>> GetUserDesignsAsync(int userId)
        {
            var designs = await _unitOfWork.Designs.GetUserDesignsAsync(userId);
            return designs.ToList();
        }

        public async Task<bool> ProcessDesignWithAIAsync(int designId)
        {
            var design = await _unitOfWork.Designs.GetByIdAsync(designId);
            if (design == null || design.IsDeleted)
                return false;

            var stopwatch = Stopwatch.StartNew();
            var log = new AIProcessingLog
            {
                DesignId = designId,
                Prompt = design.Prompt,
                RequestedAt = DateTime.UtcNow,
                ApiEndpoint = "YourAIEndpoint",
                ApiVersion = "v1"
            };

            try
            {
                design.Status = DesignStatus.Processing;
                _unitOfWork.Designs.Update(design);
                await _unitOfWork.CompleteAsync();

                // TODO: Integrate with your AI API here
                await Task.Delay(2000); // Simulate AI processing

                var generatedImagePath = Path.Combine(_uploadsPath, "generated", $"gen_{Guid.NewGuid()}.jpg");
                var generatedImageUrl = $"{_baseUrl}/uploads/generated/{Path.GetFileName(generatedImagePath)}";

                stopwatch.Stop();

                design.GeneratedImagePath = generatedImagePath;
                design.GeneratedImageUrl = generatedImageUrl;
                design.Status = DesignStatus.Completed;
                design.CompletedAt = DateTime.UtcNow;
                design.ProcessingTimeSeconds = (int)stopwatch.Elapsed.TotalSeconds;

                log.ResponseTimeMs = (int)stopwatch.Elapsed.TotalMilliseconds;
                log.RespondedAt = DateTime.UtcNow;
                log.IsSuccess = true;
                log.ResponsePayload = "Success";

                _unitOfWork.Designs.Update(design);
                await _unitOfWork.AIProcessingLogs.AddAsync(log);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                design.Status = DesignStatus.Failed;
                design.ErrorMessage = ex.Message;

                log.ResponseTimeMs = (int)stopwatch.Elapsed.TotalMilliseconds;
                log.RespondedAt = DateTime.UtcNow;
                log.IsSuccess = false;
                log.ErrorMessage = ex.Message;
                log.ErrorStackTrace = ex.StackTrace;

                _unitOfWork.Designs.Update(design);
                await _unitOfWork.AIProcessingLogs.AddAsync(log);
                await _unitOfWork.CompleteAsync();

                return false;
            }
        }

        public async Task<string> SaveImageAsync(IFormFile file, string folder)
        {
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var folderPath = Path.Combine(_uploadsPath, folder);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }

        public async Task<byte[]?> GetImageForDownloadAsync(int designId, int userId, bool isGenerated)
        {
            var design = await GetDesignByIdAsync(designId, userId);
            if (design == null)
                return null;

            var imagePath = isGenerated ? design.GeneratedImagePath : design.OriginalImagePath;

            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                return null;

            return await File.ReadAllBytesAsync(imagePath);
        }

        public async Task MarkAsDownloadedAsync(int designId)
        {
            var design = await _unitOfWork.Designs.GetByIdAsync(designId);
            if (design != null)
            {
                design.IsDownloaded = true;
                design.DownloadedAt = DateTime.UtcNow;
                _unitOfWork.Designs.Update(design);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}