using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Shatabli.Core.Application.Features.Products.Queries.GetProductImageById;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Entities;
using Shatabli.Core.Domain.Enums;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddOrignalImage
{
    public class AddDesignWithOurCeramicImageHandler : IRequestHandler<AddDesignWithOurCeramicImageCommand, AddDesignWithOurCeramicImageResponse>
    {
        private readonly IApplicationDbContext _context;
        public IStorageService _storageService;
        private readonly IGenerateRoomImageService _generateRoomImageService;
        private readonly IMediator _mediator;
        private readonly IClaimsService _claimsService;

        public AddDesignWithOurCeramicImageHandler(IApplicationDbContext context ,IStorageService storageService ,IGenerateRoomImageService generateRoomImageService ,IMediator mediator , IClaimsService claimsService)
        {
            _context = context;
            //_storageService = storageService;
            _generateRoomImageService = generateRoomImageService;
            _mediator = mediator;
            _claimsService = claimsService;
        }
        async Task<AddDesignWithOurCeramicImageResponse> IRequestHandler<AddDesignWithOurCeramicImageCommand, AddDesignWithOurCeramicImageResponse>.Handle(AddDesignWithOurCeramicImageCommand request, CancellationToken cancellationToken)
        {
            string designId = Guid.NewGuid().ToString();

            GetCeramicQuery query = new GetCeramicQuery();
            query.CeramicId = request.ceramicId;

            var ceramicImagestream = await _mediator.Send(query);

            MemoryStream ceramicMemoryStream = new MemoryStream();

            await ceramicImagestream.Stream.CopyToAsync(ceramicMemoryStream);

            var generatedImageBytes = await _generateRoomImageService.GenerateImage( request.stream, ceramicMemoryStream.ToArray(), request.designType);

            var fileName = $"{Guid.NewGuid()}.png";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp-images");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            await System.IO.File.WriteAllBytesAsync(filePath, generatedImageBytes);

            var genImagePath = $"/temp-images/{fileName}";

            Design design = new Design();
            design.Id = designId;
            design.GeneratedImagePath = genImagePath;
            design.UserId = _claimsService.GetCurrentUserId();
            //design.UserId = "bad9014b-a457-47a6-afa0-cedefa8832c0";


            _context.Designs.Add(design);
            await _context.SaveChangesAsync(cancellationToken);


            AddDesignWithOurCeramicImageResponse result = new AddDesignWithOurCeramicImageResponse();

            result.GeneratedImagePath = genImagePath;
            result.designId = designId;

            return result;
        }
    }
}
