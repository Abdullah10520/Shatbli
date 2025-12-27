using AutoMapper;
using MediatR;
using Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithUserCeramicAndPaint;
using Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithUserCeramicImage;
using Shatabli.Core.Application.Features.Products.Queries.GetProductImageById;
using Shatabli.Core.Application.Features.Subscriptions.Queries.CheckCanGenerate;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;
using Shatabli.Core.Domain.Entities;
using Shatabli.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithPaintOnly
{
    public class AddDesignWithPaintCommandHandler : IRequestHandler<AddDesignWithPaintCommand, Result<AddDesignWithPaintResponse>>
    {
        private readonly IApplicationDbContext _context;
        public IStorageService _storageService;
        private readonly IClaimsService _claimsService;
        private readonly IMediator _mediator;
        private readonly IDesignBackgroundJobService _designBackgroundJobService;
        private readonly IGenerateRoomImageService _generateRoomImageService;
        public AddDesignWithPaintCommandHandler(IApplicationDbContext context ,IGenerateRoomImageService generateRoomImageService ,IStorageService storageService, IClaimsService claimsService, IMediator mediator, IDesignBackgroundJobService designBackgroundJobService )
        {
            _context = context;
            _storageService = storageService;
            _claimsService = claimsService;
            _mediator = mediator;
            _designBackgroundJobService = designBackgroundJobService;
            _generateRoomImageService = generateRoomImageService;
        }
        public async Task<Result<AddDesignWithPaintResponse>> Handle(AddDesignWithPaintCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //var allowGenerate = await _mediator.Send(new CheckCanGenerateQuery());
                //if (!allowGenerate.IsSuccess)
                //{
                //    return Result<AddDesignWithPaintResponse>.Failure(allowGenerate.Message, allowGenerate.StatusCode, allowGenerate.Errors);
                //}

                //var US = _context.UserSubscriptions.Where(us => us.UserId == _claimsService.GetCurrentUserId()).FirstOrDefault();

                //US.ImagesGeneratedToday = US.ImagesGeneratedToday + 1;
                //US.ImagesGeneratedThisMonth = US.ImagesGeneratedThisMonth + 1;
                //_context.UserSubscriptions.Update(US);


                var generatedImageBytes = await _generateRoomImageService.GenerateDesignWithPaintOnly(request.roomBytes, request.colorCode);

                if (generatedImageBytes == null || generatedImageBytes.Length == 0)
                {
                    return Result<AddDesignWithPaintResponse>.Failure("Failed to generate image from AI service.", 500);
                }


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
                    return Result<AddDesignWithPaintResponse>.Unauthorized("User session expired or invalid.");
                }

                string designId = Guid.NewGuid().ToString();
                Design design = new Design
                {
                    Id = designId,
                    GeneratedImagePath = genImagePath,
                    UserId = currentUserId
                };

                _context.Designs.Add(design);
                await _context.SaveChangesAsync(cancellationToken);

                _designBackgroundJobService.CleanUpDb(designId);

                var response = new AddDesignWithPaintResponse
                {
                    generatedImagePath = genImagePath,
                    designId = designId
                };

                return Result<AddDesignWithPaintResponse>.Success(response, "Design created and saved localy successfully.");
            }
            catch (Exception ex)
            {
                return Result<AddDesignWithPaintResponse>.Failure(
                    message: "An unexpected error occurred during processing.",
                    statusCode: 500,
                    errors: new List<string> { ex.Message }
                );
            }
        }
    }
}
