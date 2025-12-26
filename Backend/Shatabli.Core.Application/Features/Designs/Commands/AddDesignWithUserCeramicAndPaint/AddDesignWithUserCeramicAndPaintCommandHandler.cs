using AutoMapper;
using MediatR;
using Shatabli.Core.Application.Features.Designs.Commands.AddOrignalImage;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;
using Shatabli.Core.Domain.Entities;
using Shatabli.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithUserCeramicAndPaint
{
    public class AddDesignWithUserCeramicAndPaintCommandHandler : IRequestHandler<AddDesignWithUserCeramicAndPaintCommand, Result<AddDesignWithUserCeramicAndPaintResponse>>
    {
        private readonly IApplicationDbContext _context;
        public IStorageService _storageService;
        private readonly IClaimsService _claimsService;
        private readonly IDesignBackgroundJobService _designBackgroundJobService;
        private readonly IGenerateRoomImageService _generateRoomImageService;
        public AddDesignWithUserCeramicAndPaintCommandHandler(IApplicationDbContext context, IGenerateRoomImageService generateRoomImageService, IStorageService storageService, IClaimsService claimsService, IDesignBackgroundJobService designBackgroundJobService)
        {
            _context = context;
            _storageService = storageService;
            _claimsService = claimsService;
            _designBackgroundJobService = designBackgroundJobService;
            _generateRoomImageService = generateRoomImageService;
        }
        public async Task<Result<AddDesignWithUserCeramicAndPaintResponse>> Handle(AddDesignWithUserCeramicAndPaintCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var generatedImageBytes = await _generateRoomImageService.GenerateDesignWithCeramicAndPaint(request.roomBytes, request.ceramicBytes, request.colorCode);

                if (generatedImageBytes == null || generatedImageBytes.Length == 0)
                {
                    return Result<AddDesignWithUserCeramicAndPaintResponse>.Failure(
                        message: "AI Generation Failed",
                        statusCode: 500,
                        errors: new List<string> { "The AI service returned empty or null data." }
                    );
                }

                string designId = Guid.NewGuid().ToString();
                var fileName = $"{Guid.NewGuid()}.png";
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp-images");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, fileName);
                await System.IO.File.WriteAllBytesAsync(filePath, generatedImageBytes);

                var genImagePath = $"/temp-images/{fileName}";

                var currentUserId = _claimsService.GetCurrentUserId();
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Result<AddDesignWithUserCeramicAndPaintResponse>.Unauthorized("User session expired or invalid.");
                }
                Design design = new Design
                {
                    Id = designId,
                    GeneratedImagePath = genImagePath,
                    UserId = currentUserId
                };

                _context.Designs.Add(design);
                await _context.SaveChangesAsync(cancellationToken);

                _designBackgroundJobService.CleanUpDb(designId);

                var response = new AddDesignWithUserCeramicAndPaintResponse
                {
                    generatedImagePath = genImagePath,
                    designId = designId
                };

                return Result<AddDesignWithUserCeramicAndPaintResponse>.Success(response, "Design generated and saved local successfully.");
            }
            catch (Exception ex)
            {
                return Result<AddDesignWithUserCeramicAndPaintResponse>.Failure(
                    message: "An unexpected error occurred during processing.",
                    statusCode: 500,
                    errors: new List<string> { ex.Message }
                );
            }
        }
    }
}
