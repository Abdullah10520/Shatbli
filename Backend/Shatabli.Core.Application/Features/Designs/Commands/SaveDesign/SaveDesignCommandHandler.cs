using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shatabli.Core.Application.Features.Designs.Commands.SaveDesign
{
    public class SaveDesignCommandHandler : IRequestHandler<SaveDesignCommand, SaveDesignResponse>
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

        async Task<SaveDesignResponse> IRequestHandler<SaveDesignCommand, SaveDesignResponse>.Handle(SaveDesignCommand request, CancellationToken cancellationToken)
        {
            Design designFromDB = _context.Designs.Where(d => d.Id == request.designId).FirstOrDefault();

            //string genImageUrl = await _storageService.UploadAsync(request.designImagePath, designId);
            string genImageUrl = await _storageService.UploadAsync(designFromDB.GeneratedImagePath, designFromDB.Id);

            designFromDB.GeneratedImageUrl = genImageUrl;
            designFromDB.GeneratedImagePath = null;
            designFromDB.CreatedAt = DateTime.UtcNow;

            _context.Designs.Update(designFromDB);
            await _context.SaveChangesAsync(cancellationToken);

            SaveDesignResponse response = new SaveDesignResponse();
            response.generatedImageUrl = genImageUrl;
            response.designId = designFromDB.Id;

            return response;
        }
    }
}
