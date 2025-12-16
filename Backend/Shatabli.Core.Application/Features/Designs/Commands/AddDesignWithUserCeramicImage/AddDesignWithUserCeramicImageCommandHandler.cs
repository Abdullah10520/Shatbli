using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Entities;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithUserCeramicImage
{
    public class AddDesignWithUserCeramicImageCommandHandler : IRequestHandler<AddDesignWithUserCeramicImageCommand, AddDesignWithUserCeramicImageResponse>
    {
        private readonly IApplicationDbContext _context;
        public IStorageService _storageService;
        private readonly IGenerateRoomImageService _generateRoomImageService;
        public AddDesignWithUserCeramicImageCommandHandler(IApplicationDbContext context ,IGenerateRoomImageService generateRoomImageService ,IStorageService storageService )
        {
            _context = context;
            _storageService = storageService;
            _generateRoomImageService = generateRoomImageService;
        }
        public async Task<AddDesignWithUserCeramicImageResponse> Handle(AddDesignWithUserCeramicImageCommand request, CancellationToken cancellationToken)
        {
            string designId = Guid.NewGuid().ToString();

            var roomImageUrl = await _storageService.Upload(new MemoryStream(request.roomBytes), request.roomimageName, designId);

            var productImageUrl = await _storageService.Upload(new MemoryStream( request.ceramicOrPaintBytes), request.ceramicOrPaintimageName, designId+" Product");
            

            var GeneratedImageBytes = await _generateRoomImageService.GenerateImage(request.roomBytes, request.ceramicOrPaintBytes, request.designType);

            var genImageUrl = "";

            using (var genstream = new MemoryStream(GeneratedImageBytes))
            {
                genImageUrl = await _storageService.Upload(genstream, "Ai Generted "+request.roomimageName , designId + "-AiGen");
            }

            var genImgStream = new MemoryStream(GeneratedImageBytes);

            AddDesignWithUserCeramicImageResponse response = new AddDesignWithUserCeramicImageResponse();
            response.GeneratedImage = genImgStream;


            Design design = new Design();
            design.Id = designId;
            design.UserId = "4";
            design.OriginalImageUrl = roomImageUrl;
            design.GeneratedImageUrl = genImageUrl;
            design.ProductImageUrl = productImageUrl;
            design.CompletedAt = DateTime.UtcNow;

            await _context.Designs.AddAsync(design, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return response;


        }
    }
}
