using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Interfaces
{
    public interface IStorageService
    {
        public Task<string> Upload(Stream stream, string imageName, string imagePublicId);
        public string GetImageURL(string imagePublicId);
        public Task<Stream> downloadImageStream(string imagePublicId);

    }
}
