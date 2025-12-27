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

        public async Task<byte[]> GenerateDesignWithCeramicAndPaint(byte[] roomImage, byte[] ceramicImage, string colorCode)
        {
            var form = new MultipartFormDataContent();

            // Room image
            var roomContent = new ByteArrayContent(roomImage);
            roomContent.Headers.ContentType =
                MediaTypeHeaderValue.Parse("image/jpeg");
            form.Add(roomContent, "roomImage", "room.jpg");

            // Ceramic tile image
            var ceramicContent = new ByteArrayContent(ceramicImage);
            ceramicContent.Headers.ContentType =
                MediaTypeHeaderValue.Parse("image/jpeg");
            form.Add(ceramicContent, "ceramicTileImage", "tile.jpg");

            // Wall color HEX code
            var colorContent = new StringContent(colorCode);
            form.Add(colorContent, "wall_color_hex");

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync(
                    "/roomCeramicWithWallColor",
                    form);

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsByteArrayAsync();
            }
            finally
            {
                form.Dispose();
            }
        }

        public async Task<byte[]> GenerateImage(byte[] roomImage, byte[] ceramicOrPaintImage , DesignType designType)
        {
            var form = new MultipartFormDataContent();
            
            // Room image
            var roomContent = new ByteArrayContent(roomImage);
            roomContent.Headers.ContentType =
                MediaTypeHeaderValue.Parse("image/jpeg");

            // Ceramic / paint image
            var ceramicOrPaintContent = new ByteArrayContent(ceramicOrPaintImage);
            ceramicOrPaintContent.Headers.ContentType =
                MediaTypeHeaderValue.Parse("image/jpeg");

            form.Add(roomContent, "roomImage", "room.jpg");

            form.Add(ceramicOrPaintContent, (designType == DesignType.CeramicFloor ? "ceramicTileImage" : "wallPaintingImage"), "tile.jpg");

            HttpResponseMessage response;
            // Determine endpoint based on designType
            string endpoint = designType switch
            {
                DesignType.CeramicFloor => "/roomCeramic",
                DesignType.WallPaint => "/roomWall",
                // Add more mappings as needed
                _ => "/roomCeramic"
            };
            try
            {
                response = await _httpClient.PostAsync(endpoint, form);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsByteArrayAsync();
            }
            finally
            {
                // Safe to dispose now (no streams involved)
                form.Dispose();
            }
        }

        public async Task<byte[]> GenerateDesignWithPaintOnly(byte[] roomImage, string colorCode)
        {
            var form = new MultipartFormDataContent();

            // Room image
            var roomContent = new ByteArrayContent(roomImage);
            roomContent.Headers.ContentType =
                MediaTypeHeaderValue.Parse("image/jpeg");
            form.Add(roomContent, "roomImage", "room.jpg");


            // Wall color HEX code
            var colorContent = new StringContent(colorCode);
            form.Add(colorContent, "wall_color_hex");

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync(
                    "/roomPaintOnly",
                    form);

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsByteArrayAsync();
            }
            finally
            {
                form.Dispose();
            }
        }
    }
}
