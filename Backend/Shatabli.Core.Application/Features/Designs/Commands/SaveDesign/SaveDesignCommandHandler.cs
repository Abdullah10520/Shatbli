using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;
using Shatabli.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shatabli.Core.Application.Features.Designs.Commands.SaveDesign
{
    public class SaveDesignCommandHandler : IRequestHandler<SaveDesignCommand, Result<SaveDesignResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStorageService _storageService;
        private readonly IClaimsService _claimsService;

        public SaveDesignCommandHandler(IApplicationDbContext context, IStorageService storageService, IClaimsService claimsService)
        {
            _context = context;
            _storageService = storageService;
            _claimsService = claimsService;
        }

        async Task<Result<SaveDesignResponse>> IRequestHandler<SaveDesignCommand, Result<SaveDesignResponse>>.Handle(SaveDesignCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var designFromDB = await _context.Designs
                    .FirstOrDefaultAsync(d => d.Id == request.designId);


                if (designFromDB == null)
                {
                    return Result<SaveDesignResponse>.NotFound($"Design with ID {request.designId} not found.");
                }

                if (string.IsNullOrEmpty(designFromDB.GeneratedImagePath))
                {
                    return Result<SaveDesignResponse>.Failure("Design is already saved or temp image is missing.", 400);
                }

                string genImageUrl = await _storageService.UploadAsync(designFromDB.GeneratedImagePath, designFromDB.Id);

                if (string.IsNullOrEmpty(genImageUrl))
                {
                    return Result<SaveDesignResponse>.Failure("Failed to upload image to storage service.", 500);
                }

                designFromDB.GeneratedImageUrl = genImageUrl;
                designFromDB.GeneratedImagePath = null;
                designFromDB.CreatedAt = DateTime.UtcNow;

                _context.Designs.Update(designFromDB);
                await _context.SaveChangesAsync(cancellationToken);

                var response = new SaveDesignResponse
                {
                    generatedImageUrl = genImageUrl,
                    designId = designFromDB.Id
                };

                return Result<SaveDesignResponse>.Success(response, "Design saved successfully to cloud storage.");
            }
            catch (Exception ex)
            {
                return Result<SaveDesignResponse>.Failure($"An error occurred while saving the design: {ex.Message}", 500);
            }
        }
    }
}
