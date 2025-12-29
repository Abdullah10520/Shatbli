using AutoMapper;
using MediatR;
using Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithUserCeramicAndPaint;
using Shatabli.Core.Application.Features.Designs.Commands.AddOrignalImage;
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

namespace Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithOurCeramicAndPaint
{
    public class AddDesignWithOurCeramicAndPaintCommandHandler : IRequestHandler<AddDesignWithOurCeramicAndPaintCommand, Result<AddDesignWithOurCeramicAndPaintResponse>>
    {
        private readonly IApplicationDbContext _context;
        public IStorageService _storageService;
        private readonly IClaimsService _claimsService;
        private readonly IMediator _mediator;
        private readonly IDesignBackgroundJobService _designBackgroundJobService;
        private readonly IGenerateRoomImageService _generateRoomImageService;
        public AddDesignWithOurCeramicAndPaintCommandHandler(IApplicationDbContext context ,IGenerateRoomImageService generateRoomImageService ,IStorageService storageService, IClaimsService claimsService, IMediator mediator, IDesignBackgroundJobService designBackgroundJobService )
        {
            _context = context;
            _storageService = storageService;
            _claimsService = claimsService;
            _mediator = mediator;
            _designBackgroundJobService = designBackgroundJobService;
            _generateRoomImageService = generateRoomImageService;
        }
        public async Task<Result<AddDesignWithOurCeramicAndPaintResponse>> Handle(AddDesignWithOurCeramicAndPaintCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ceramicResult = await _mediator.Send(new GetCeramicQuery { CeramicId = request.ceramicId }, cancellationToken);

                if (!ceramicResult.IsSuccess)
                {
                    return Result<AddDesignWithOurCeramicAndPaintResponse>.Failure(ceramicResult.Message, ceramicResult.StatusCode, ceramicResult.Errors);
                }

                using var ceramicMemoryStream = new MemoryStream();
                await ceramicResult.Data.Stream.CopyToAsync(ceramicMemoryStream);

                var genImgRequest = new AddDesignWithUserCeramicAndPaintCommand
                {
                    roomBytes = request.roomBytes,
                    roomimageName = request.roomimageName,
                    ceramicBytes = ceramicMemoryStream.ToArray(),
                    ceramicImageName = "ceramic.png",
                    colorCode = request.colorCode
                };

                var executionResult = await _mediator.Send(genImgRequest, cancellationToken);

                if (!executionResult.IsSuccess)
                {
                    return Result<AddDesignWithOurCeramicAndPaintResponse>.Failure(executionResult.Message, executionResult.StatusCode, executionResult.Errors.Values.SelectMany(x => x).ToList());
                }

                var response = new AddDesignWithOurCeramicAndPaintResponse
                {
                    generatedImagePath = executionResult.Data.generatedImagePath,
                    designId = executionResult.Data.designId
                };

                return Result<AddDesignWithOurCeramicAndPaintResponse>.Success(response, "Design generated successfully using selected product.");
            }
            catch (Exception ex)
            {
                return Result<AddDesignWithOurCeramicAndPaintResponse>.Failure($"Unexpected error: {ex.Message}", 500);
            }
        }
    }
}
