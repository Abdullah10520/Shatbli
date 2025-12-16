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

        public AddDesignWithOurCeramicImageHandler(IApplicationDbContext context ,IStorageService storageService ,IGenerateRoomImageService generateRoomImageService ,IMediator mediator)
        {
            _context = context;
            _storageService = storageService;
            _generateRoomImageService = generateRoomImageService;
            _mediator = mediator;
        }
        async Task<AddDesignWithOurCeramicImageResponse> IRequestHandler<AddDesignWithOurCeramicImageCommand, AddDesignWithOurCeramicImageResponse>.Handle(AddDesignWithOurCeramicImageCommand request, CancellationToken cancellationToken)
        {
            string designId = Guid.NewGuid().ToString();

            var cloudinaryImageUrl = await _storageService.Upload(new MemoryStream(request.stream) , request.imageName, designId);

            GetCeramicQuery query = new GetCeramicQuery();
            query.CeramicId = request.ceramicId;

            var ceramicImagestream = await _mediator.Send(query);

            MemoryStream ceramicMemoryStream = new MemoryStream();

            await ceramicImagestream.Stream.CopyToAsync(ceramicMemoryStream);

            var generatedImageBytes = await _generateRoomImageService.GenerateImage( request.stream, ceramicMemoryStream.ToArray(), request.designType);

            
            var GeneratedImageURL = "";

            using (var uploadStream = new MemoryStream(generatedImageBytes))
            {
                GeneratedImageURL = await _storageService.Upload(uploadStream, "Ai Generated" + request.imageName, designId+ "-AiGen");
            }

            var ceramicImageUrl = _storageService.GetImageURL(request.ceramicId);


            AddDesignWithOurCeramicImageResponse result = new AddDesignWithOurCeramicImageResponse();

            var streamToReturn = new MemoryStream(generatedImageBytes);

            result.GeneratedImage = streamToReturn;

            //Saving data in database

            Design design = new Design();
            design.Id = designId;
            design.UserId = "4";
            design.OriginalImageUrl = cloudinaryImageUrl;
            design.GeneratedImageUrl = GeneratedImageURL;
            design.ProductImageUrl = ceramicImageUrl;
            design.CompletedAt = DateTime.UtcNow;

            await _context.Designs.AddAsync(design, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
