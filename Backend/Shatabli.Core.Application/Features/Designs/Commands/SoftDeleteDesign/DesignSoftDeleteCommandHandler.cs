using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Designs.Commands.SoftDeleteDesign
{
    public class DesignSoftDeleteCommandHandler : IRequestHandler<DesignSoftDeleteCommand, Result<DesignSoftDeleteResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IClaimsService _claimsService;

        public DesignSoftDeleteCommandHandler(IApplicationDbContext context, IClaimsService claimsService)
        {
            _context = context;
            _claimsService = claimsService;
        }
        async Task<Result<DesignSoftDeleteResponse>> IRequestHandler<DesignSoftDeleteCommand, Result<DesignSoftDeleteResponse>>.Handle(DesignSoftDeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _claimsService.GetCurrentUserId();

                var designFromDb = await _context.Designs
                    .AsTracking()
                    .SingleOrDefaultAsync(d => d.Id == request.designId && d.UserId == currentUserId, cancellationToken);

                if (designFromDb == null)
                {
                    return Result<DesignSoftDeleteResponse>.NotFound("Design not found or you don't have permission to delete it.");
                }

                designFromDb.IsDeleted = true;
                designFromDb.DeletedAt = DateTime.UtcNow;
                designFromDb.DeletedBy = currentUserId;

                await _context.SaveChangesAsync(cancellationToken);

                return Result<DesignSoftDeleteResponse>.Success(new DesignSoftDeleteResponse { Success = true }, "Design deleted successfully.");
            }
            catch (Exception ex)
            {
                return Result<DesignSoftDeleteResponse>.Failure($"An error occurred while deleting: {ex.Message}", 500);
            }
        }
    }
}
