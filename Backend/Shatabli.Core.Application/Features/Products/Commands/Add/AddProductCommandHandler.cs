//using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Entities;


namespace Shatabli.Core.Application.Features.Products.Commands.Add
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, int> //: IRequestHandler<AddProductCommand ,AddProductCommandResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AddProductCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
