using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithUserCeramicImage
{
    public class AddDesignWithUserCeramicImageCommandHandler : IRequestHandler<AddDesignWithUserCeramicImageCommand, AddDesignWithUserCeramicImageResponse>
    {
        public Task<AddDesignWithUserCeramicImageResponse> Handle(AddDesignWithUserCeramicImageCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
