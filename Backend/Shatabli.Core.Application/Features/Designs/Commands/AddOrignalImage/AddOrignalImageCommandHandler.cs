using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Entities;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddOrignalImage
{
    public class AddOrignalImageCommandHandler : IRequestHandler<AddOrignalImageCommand, AddOrignalImageCommandResponse>
    {
        private readonly IApplicationDbContext _context;
        public IStorageService _storageService;
        private readonly IGenerateRoomImageService _generateRoomImageService;

        public AddOrignalImageCommandHandler(IApplicationDbContext context ,IStorageService storageService ,IGenerateRoomImageService generateRoomImageService)
        {
            _context = context;
            _storageService = storageService;
            _generateRoomImageService = generateRoomImageService;
        }
        async Task<AddOrignalImageCommandResponse> IRequestHandler<AddOrignalImageCommand, AddOrignalImageCommandResponse>.Handle(AddOrignalImageCommand request, CancellationToken cancellationToken)
        {
            var cloudinaryImageUrl = await _storageService.Upload(request.stream , request.ImageName);

            var ceramicImageStream = await _storageService.downloadImageStream(request.CeramicId);

            var generatedImageBytes = await _generateRoomImageService.GenerateImageWithCeramic(request.stream, ceramicImageStream);

            var GeneratedImageURL = "";

            using (var uploadStream = new MemoryStream(generatedImageBytes))
            {
                GeneratedImageURL = await _storageService.Upload(uploadStream, request.ImageName);
            }

            var ceramicImageUrl = _storageService.GetImageURL(request.CeramicId);

            //var GeneratedImageURL = await _storageService.Upload(generatedImageStream, request.ImageName);

            //generatedImageStream.Seek(0, SeekOrigin.Begin);

            //var ceramicImage = File(ceramicImageStream, "image/png");

            AddOrignalImageCommandResponse result = new AddOrignalImageCommandResponse();
            //result.ImageURL = cloudinaryImageUrl;

            //get product URL From Cloudinary
            //string productURL = _storageService.GetImageURL("4");
            //error That You Should Remove
            //result.ImageURL = productURL;

            var streamToReturn = new MemoryStream(generatedImageBytes);

            result.GeneratedImage = streamToReturn;

            //await Task.WhenAll(cloudinaryImageUrl, GeneratedImageURL);

            Design design = new Design();
            design.UserId = 4;
            design.OriginalImageUrl = cloudinaryImageUrl;
            design.GeneratedImageUrl = GeneratedImageURL;
            design.CeramicImageUrl = ceramicImageUrl;

            await _context.Designs.AddAsync(design, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
