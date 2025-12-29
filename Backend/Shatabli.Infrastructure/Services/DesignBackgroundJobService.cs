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
        private readonly IPathProvider _pathProvider;

        public DesignBackgroundJobService(IApplicationDbContext context, IPathProvider pathProvider) 
        {
            _context = context;
            _pathProvider = pathProvider;
        }
        public void CleanUpDb(string designId)
        {
            Hangfire.BackgroundJob.Schedule(() => CleanDesignAsync(designId), TimeSpan.FromMinutes(5));
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

            _context.Designs.Remove(design);
            await _context.SaveChangesAsync();

            var fileName = Path.GetFileName(design.GeneratedImagePath.TrimStart('/')); // Trim لضمان عدم وجود سلاش في البداية
            var fullPath = Path.Combine(_pathProvider.WebRootPath, "temp-images", fileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
