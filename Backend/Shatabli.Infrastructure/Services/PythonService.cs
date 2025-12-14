using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shatabli.Core.Application.Interfaces;

namespace Shatabli.Infrastructure.Services
{
    public class PythonService : IGenerateRoomImageService
    {
        private readonly IStorageService _storageService;

        public PythonService(IStorageService storageService) 
        {
            _storageService = storageService;
        }
        public async Task<byte[]> GenerateImageWithCeramic(Stream roomImage, Stream ceramicImage)
        {
            var image = await _storageService.downloadImageStream("screencapture-elite-car-vue-lovable-app-2025-11-26-22_08_22");


                        
            
            var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);

            return memoryStream.ToArray();
        }
    }
}
