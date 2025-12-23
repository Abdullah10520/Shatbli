using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Entities;
using Shatabli.Core.Domain.Enums;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithUserCeramicAndPaint
{
    public class AddDesignWithUserCeramicAndPaintCommandHandler : IRequestHandler<AddDesignWithUserCeramicAndPaintCommand, AddDesignWithUserCeramicAndPaintResponse>
    {
        private readonly IApplicationDbContext _context;
        public IStorageService _storageService;
        private readonly IClaimsService _claimsService;
        private readonly IGenerateRoomImageService _generateRoomImageService;
        public AddDesignWithUserCeramicAndPaintCommandHandler(IApplicationDbContext context ,IGenerateRoomImageService generateRoomImageService ,IStorageService storageService, IClaimsService claimsService )
        {
            _context = context;
            _storageService = storageService;
            _claimsService = claimsService;
            _generateRoomImageService = generateRoomImageService;
        }
        public async Task<AddDesignWithUserCeramicAndPaintResponse> Handle(AddDesignWithUserCeramicAndPaintCommand request, CancellationToken cancellationToken)
        {
            string designId = Guid.NewGuid().ToString();
            var GeneratedImageBytes = await _generateRoomImageService.GenerateDesignWithCeramicAndPaint(request.roomBytes, request.ceramicBytes, request.colorCode);
            //var GeneratedImageBytes = await _generateRoomImageService.GenerateImage(request.roomBytes, request.ceramicBytes, DesignType.CeramicFloor);


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


            _context.Designs.Add(design);
            await _context.SaveChangesAsync(cancellationToken);


            AddDesignWithUserCeramicAndPaintResponse response = new AddDesignWithUserCeramicAndPaintResponse();
            response.GeneratedImageUrl = genImagePath;
            response.designId = designId;

            return response;
        }
    }
}
