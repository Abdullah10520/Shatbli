using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shatabli.Core.Domain.Enums;

namespace Shatabli.Core.Application.Interfaces
{
    public interface IGenerateRoomImageService
    {
        public Task<byte[]> GenerateImage(Stream roomImage, Stream ceramicImage, DesignType designType);
    }
}
