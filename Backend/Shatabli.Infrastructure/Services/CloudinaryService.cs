using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Entities;

namespace Shatabli.Infrastructure.Services
{
    public class CloudinaryService : IStorageService
    {
        public Account account;
        public Cloudinary cloudinary;
        private HttpClient _httpClient;
        public CloudinaryService(HttpClient httpClient) 
        {
            account = new Account(
                "dymvpdlzd",                            //Cloud Name
                "692575487955145",                      //API Key
                "HnUXYn0IeMrCyQg__XRNfKCl0bo"           //API Secret
                );
            cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;

            _httpClient = httpClient;

        }
        public async Task<string> Upload(Stream stream, string imageName, string imagePublicId)
        {
            var uploadparams = new ImageUploadParams()
            {
                File = new FileDescription(imageName, stream),
                PublicId = imagePublicId,
                Overwrite = false
            };

            var uploadResult = await cloudinary.UploadAsync(uploadparams);
            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }
            return "Can't Save The Image";
        }

        public string GetImageURL(string imagePublicId)
        {
            string publicId = imagePublicId.ToString();
            var urlBuilder = cloudinary.Api.UrlImgUp.Format("jpg");
            return urlBuilder.BuildUrl(publicId);
        }

        public async Task<Stream> downloadImageStream(string imagePublicId)
        {
            var downloadUrl = GetImageURL(imagePublicId);
            //var urlBuilder = cloudinary.Api.UrlImgUp;
            
            //var downloadUrl = urlBuilder.BuildUrl($"OrignalImage/{imagePublicId}");
            //var downloadUrl = urlBuilder.BuildUrl(imagePublicId);

            var response = await _httpClient.GetAsync(downloadUrl);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();

        }
    }
    
}
