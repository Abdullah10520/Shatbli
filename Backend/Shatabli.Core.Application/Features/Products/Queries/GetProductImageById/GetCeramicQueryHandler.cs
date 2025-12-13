using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Shatabli.Core.Application.Interfaces;

namespace Shatabli.Core.Application.Features.Products.Queries.GetProductImageById
{
    public class GetCeramicQueryHandler : IRequestHandler<GetCeramicQuery, GetCeramicResponse>
    {
        private readonly IStorageService _storageService;

        public GetCeramicQueryHandler(IStorageService storageService)
        {
            _storageService = storageService;
        }
        async Task<GetCeramicResponse> IRequestHandler<GetCeramicQuery, GetCeramicResponse>.Handle(GetCeramicQuery request, CancellationToken cancellationToken)
        {
            var imageStream = await _storageService.downloadImageStream(request.CeramicId);
            GetCeramicResponse response = new GetCeramicResponse();
            response.Stream = imageStream;

            return response;
        }
    }
}
