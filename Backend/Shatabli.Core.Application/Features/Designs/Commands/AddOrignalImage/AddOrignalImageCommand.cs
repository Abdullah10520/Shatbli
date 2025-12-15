using MediatR;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddOrignalImage
{
    public class AddOrignalImageCommand : IRequest<AddOrignalImageCommandResponse>
    {
        //public IFormFile imageFile { get; set; }
        public Stream stream { get; set; }
        public string ImageName { get; set; }

        public string CeramicId { get; set; }
    }
}
