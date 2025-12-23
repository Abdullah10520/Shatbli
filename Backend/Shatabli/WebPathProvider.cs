using Shatabli.Core.Application.Interfaces;

namespace Shatabli.API
{
    public class WebPathProvider : IPathProvider
    {
        private readonly IWebHostEnvironment _env;

        public WebPathProvider(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string WebRootPath => _env.WebRootPath;
    }
}
