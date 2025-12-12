using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Shatabli.Core.Application.Interfaces;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddOrignalImage
{
    public class AddOrignalImageCommandHandler : IRequestHandler<AddOrignalImageCommand, AddOrignalImageCommandResponse>
    {
        public IStorageService _storageService;
        public AddOrignalImageCommandHandler(IStorageService storageService)
        {
            _storageService = storageService;
        }
        async Task<AddOrignalImageCommandResponse> IRequestHandler<AddOrignalImageCommand, AddOrignalImageCommandResponse>.Handle(AddOrignalImageCommand request, CancellationToken cancellationToken)
        {
            var cloudinaryImageUrl = await _storageService.Upload(request.stream , request.ImageName);
            AddOrignalImageCommandResponse result = new AddOrignalImageCommandResponse();
            result.ImageURL = cloudinaryImageUrl;
            return result;
        }
    }
}
