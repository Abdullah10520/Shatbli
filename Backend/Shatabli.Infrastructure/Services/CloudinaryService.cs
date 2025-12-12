using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Shatabli.Core.Application.Interfaces;

namespace Shatabli.Infrastructure.Services
{
    public class CloudinaryService : IStorageService
    {
        public Account account;
        public Cloudinary cloudinary;
        public CloudinaryService() 
        {
            account = new Account(
                "dymvpdlzd",                            //Cloud Name
                "692575487955145",                      //API Key
                "HnUXYn0IeMrCyQg__XRNfKCl0bo"           //API Secret
                );
            cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;

        }
        public async Task<string> Upload(Stream stream, string ImageName)
        {
            var uploadparams = new ImageUploadParams()
            {
                File = new FileDescription(ImageName, stream),
                PublicId = Path.GetFileNameWithoutExtension(ImageName),
                Overwrite = true
            };

            var uploadResult = await cloudinary.UploadAsync(uploadparams);
            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }
            return "Can't Save The Image";
        }
    }
}
