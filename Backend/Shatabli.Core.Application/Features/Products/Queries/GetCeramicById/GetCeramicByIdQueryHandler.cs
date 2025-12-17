using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Enums;

namespace Shatabli.Core.Application.Features.Products.Queries.GetCeramicById
{
    public class GetCeramicByIdQueryHandler : IRequestHandler<GetCeramicByIdQuery, GetCeramicByIdResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCeramicByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        async Task<GetCeramicByIdResponse> IRequestHandler<GetCeramicByIdQuery, GetCeramicByIdResponse>.Handle(GetCeramicByIdQuery request, CancellationToken cancellationToken)
        {

            var result = _context.Products
                .Where(p => p.Id == request.ceramicId && p.Category == ProductCategory.FlooringCeramics)
                .Select(d => new CeramicDTO
                {
                    Id = d.Id,
                    ImageUrl = d.ImageUrl,
                    IsActive = d.IsActive,
                    Name = d.Name
                }).FirstOrDefault();

            GetCeramicByIdResponse response = new();
            response.ceramicProduct = result ;

            return response;
        }
    }
}
