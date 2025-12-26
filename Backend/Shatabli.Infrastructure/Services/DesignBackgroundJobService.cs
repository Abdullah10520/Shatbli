using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Infrastructure.Services
{
    public class DesignBackgroundJobService : IDesignBackgroundJobService
    {
        private readonly IApplicationDbContext _context;

        public DesignBackgroundJobService(IApplicationDbContext context) 
        {
            _context = context;
        }
        public void CleanUpDb(string designId)
        {
            Hangfire.BackgroundJob.Schedule(() => CleanDesignAsync(designId), TimeSpan.FromMinutes(1));
        }
        public async Task CleanDesignAsync(string designId)
        {
            Console.WriteLine("background job is applying");

            var design = await _context.Designs
                .IgnoreQueryFilters()
                .Where(d=>d.GeneratedImagePath!=null)
                .FirstOrDefaultAsync(d => d.Id == designId);

            if (design == null)
                return;

            design.IsDeleted = true;
            design.DeletedAt = DateTime.UtcNow;

            _context.Designs.Attach(design);

            var dbContext = _context as DbContext;

            dbContext.Entry(design).Property(x => x.IsDeleted).IsModified = true;
            dbContext.Entry(design).Property(x => x.DeletedAt).IsModified = true;

            await _context.SaveChangesAsync();
        }
    }
}
