using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;

namespace Shatabli.Core.Application.Features.Designs.Commands.SoftDeleteDesign
{
    public class DesignSoftDeleteCommandHandler : IRequestHandler<DesignSoftDeleteCommand, DesignSoftDeleteResponse>
    {
        private readonly IApplicationDbContext _context;

        public DesignSoftDeleteCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        async Task<DesignSoftDeleteResponse> IRequestHandler<DesignSoftDeleteCommand, DesignSoftDeleteResponse>.Handle(DesignSoftDeleteCommand request, CancellationToken cancellationToken)
        { 
           var designFromDb = _context.Designs
                .Where(d => d.Id == request.designId).AsTracking().FirstOrDefault();
            //designFromDb.IsDeleted = true;

            _context.Designs.Remove(designFromDb);
            await _context.SaveChangesAsync(cancellationToken);

            DesignSoftDeleteResponse response = new();
            response.Success = true;
            return response;
            //throw new NotImplementedException();
        }
    }
}
