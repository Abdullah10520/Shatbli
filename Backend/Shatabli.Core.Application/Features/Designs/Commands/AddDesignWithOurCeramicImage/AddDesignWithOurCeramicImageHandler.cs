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

namespace Shatabli.Core.Application.Features.Designs.Commands.AddOrignalImage
{
    public class AddDesignWithOurCeramicImageHandler : IRequestHandler<AddDesignWithOurCeramicImageCommand, Result<AddDesignWithOurCeramicImageResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IGenerateRoomImageService _generateRoomImageService;
        private readonly IMediator _mediator;
        private readonly IClaimsService _claimsService;
        private readonly IDesignBackgroundJobService _designBackgroundJobService;

        public AddDesignWithOurCeramicImageHandler(IApplicationDbContext context, IGenerateRoomImageService generateRoomImageService ,IMediator mediator , IClaimsService claimsService, IDesignBackgroundJobService designBackgroundJobService)
        {
            _context = context;
            _generateRoomImageService = generateRoomImageService;
            _mediator = mediator;
            _claimsService = claimsService;
            _designBackgroundJobService = designBackgroundJobService;
        }
        async Task<Result<AddDesignWithOurCeramicImageResponse>> IRequestHandler<AddDesignWithOurCeramicImageCommand, Result<AddDesignWithOurCeramicImageResponse>>.Handle(AddDesignWithOurCeramicImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var allowGenerate = await _mediator.Send(new CheckCanGenerateQuery());
                if (!allowGenerate.IsSuccess)
                {
                    return Result<AddDesignWithOurCeramicImageResponse>.Failure(allowGenerate.Message, allowGenerate.StatusCode, allowGenerate.Errors);
                }


                var ceramicResult = await _mediator.Send(new GetCeramicQuery { CeramicId = request.ceramicId }, cancellationToken);

                if (!ceramicResult.IsSuccess)
                {
                    return Result<AddDesignWithOurCeramicImageResponse>.Failure(ceramicResult.Message, ceramicResult.StatusCode, ceramicResult.Errors);
                }

                using var ceramicMemoryStream = new MemoryStream();
                await ceramicResult.Data.Stream.CopyToAsync(ceramicMemoryStream);

                var generatedImageBytes = await _generateRoomImageService.GenerateImage(request.roomBytes, ceramicMemoryStream.ToArray(), request.designType);

                if (generatedImageBytes == null || generatedImageBytes.Length == 0)
                {
                    return Result<AddDesignWithOurCeramicImageResponse>.Failure("AI service failed to generate image.", 500);
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
                    return Result<AddDesignWithOurCeramicImageResponse>.Unauthorized("User session expired or invalid.");
                }
                Design design = new Design
                {
                    Id = designId,
                    GeneratedImagePath = genImagePath,
                    UserId = currentUserId
                };

                var US = _context.UserSubscriptions.Where(us => us.UserId == _claimsService.GetCurrentUserId()).FirstOrDefault();

                US.ImagesGeneratedToday = US.ImagesGeneratedToday + 1;
                US.ImagesGeneratedThisMonth = US.ImagesGeneratedThisMonth + 1;
                _context.UserSubscriptions.Update(US);

                _context.Designs.Add(design);
                await _context.SaveChangesAsync(cancellationToken);

                
                _designBackgroundJobService.CleanUpDb(designId);

                var result = new AddDesignWithOurCeramicImageResponse
                {
                    GeneratedImagePath = genImagePath,
                    designId = designId
                };

                return Result<AddDesignWithOurCeramicImageResponse>.Success(result, "Design created successfully using our product.");
            }
            catch (Exception ex)
            {
                return Result<AddDesignWithOurCeramicImageResponse>.Failure(
                    message: "An unexpected error occurred during processing.",
                    statusCode: 500,
                    errors: new List<string> { ex.Message }
                );
            }
        }
    }
}
