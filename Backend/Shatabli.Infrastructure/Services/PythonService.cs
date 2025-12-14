using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Enums;
using Microsoft.Identity.Client;

namespace Shatabli.Infrastructure.Services
{
    public class PythonService : IGenerateRoomImageService
    {
        private readonly IStorageService _storageService;
        private readonly HttpClient _httpClient;
        public PythonService(IStorageService storageService, IHttpClientFactory httpClientFactory) 
        {
            _storageService = storageService;
            _httpClient = httpClientFactory.CreateClient("AIService");
        }
        public async Task<byte[]> GenerateImage(byte[] roomImage, byte[] ceramicImage , DesignType designType)
        {
            var form = new MultipartFormDataContent();
            
            // Room image
            var roomContent = new ByteArrayContent(roomImage);
            roomContent.Headers.ContentType =
                MediaTypeHeaderValue.Parse("image/jpeg");

            // Ceramic / paint image
            var ceramicContent = new ByteArrayContent(ceramicImage);
            ceramicContent.Headers.ContentType =
                MediaTypeHeaderValue.Parse("image/jpeg");

            form.Add(roomContent, "roomImage", "room.jpg");
            form.Add(ceramicContent, "ceramicTileImage", "tile.jpg");

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync("/roomCeramic", form);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsByteArrayAsync();
            }
            finally
            {
                // Safe to dispose now (no streams involved)
                form.Dispose();
            }
        }
    }
}
