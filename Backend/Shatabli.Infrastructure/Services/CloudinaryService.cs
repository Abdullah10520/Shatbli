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
        private readonly IPathProvider _pathProvider;
        public CloudinaryService(HttpClient httpClient, IPathProvider pathProvider) 
        {
            account = new Account(
                "dymvpdlzd",                            //Cloud Name
                "692575487955145",                      //API Key
                "HnUXYn0IeMrCyQg__XRNfKCl0bo"           //API Secret
                );
            cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;

            _httpClient = httpClient;
            _pathProvider = pathProvider;

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

        public async Task<string> UploadAsync(string imageUrl,string imagePublicId)
        {

            var fileName = Path.GetFileName(imageUrl.TrimStart('/')); // Trim لضمان عدم وجود سلاش في البداية
            var fullPath = Path.Combine(_pathProvider.WebRootPath, "temp-images", fileName);

            if (!File.Exists(fullPath))
            {
                return null;
            }

            try
            {
                using (var stream = System.IO.File.OpenRead(fullPath))
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(imagePublicId, stream),
                        PublicId = imagePublicId,
                        Overwrite = true 
                    };

                    var uploadResult = await cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.Error == null) 
                    {
                        stream.Close(); 
                        if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                        }

                        return uploadResult.SecureUrl.ToString();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        public string GetImageURL(string imagePublicId)
        {
            string publicId = imagePublicId.ToString();
            var urlBuilder = cloudinary.Api.UrlImgUp.Format("jpg");
            return urlBuilder.BuildUrl(publicId);
        }

        public async Task<Stream> downloadImageStream(string imagePublicId)
        {
            try
            {
                var downloadUrl = GetImageURL(imagePublicId);
                var response = await _httpClient.GetAsync(downloadUrl);

                if (!response.IsSuccessStatusCode)
                    return null; 

                return await response.Content.ReadAsStreamAsync();
            }
            catch
            {
                return null; 
            }
        }
    }
    
}
