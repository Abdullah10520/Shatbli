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
        private readonly IClaimsService _claimsService;
        private readonly IGenerateRoomImageService _generateRoomImageService;
        public AddDesignWithUserCeramicImageCommandHandler(IApplicationDbContext context ,IGenerateRoomImageService generateRoomImageService ,IStorageService storageService, IClaimsService claimsService )
        {
            _context = context;
            _storageService = storageService;
            _claimsService = claimsService;
            _generateRoomImageService = generateRoomImageService;
        }
        public async Task<AddDesignWithUserCeramicImageResponse> Handle(AddDesignWithUserCeramicImageCommand request, CancellationToken cancellationToken)
        {
            string designId = Guid.NewGuid().ToString();

            var GeneratedImageBytes = await _generateRoomImageService.GenerateImage(request.roomBytes, request.ceramicOrPaintBytes, request.designType);


            var fileName = $"{Guid.NewGuid()}.png";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp-images");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            await System.IO.File.WriteAllBytesAsync(filePath, GeneratedImageBytes);

            var genImagePath = $"/temp-images/{fileName}";


            Design design = new Design();
            design.Id = designId;
            design.GeneratedImagePath = genImagePath;
            design.UserId = _claimsService.GetCurrentUserId();
            //design.UserId = "bad9014b-a457-47a6-afa0-cedefa8832c0";



            _context.Designs.Add(design);
            await _context.SaveChangesAsync(cancellationToken);

            AddDesignWithUserCeramicImageResponse response = new AddDesignWithUserCeramicImageResponse();
            response.GeneratedImagePath = genImagePath;
            response.designId = designId;

            return response;
        }
    }
}
