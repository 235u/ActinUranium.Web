using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;

namespace ActinUranium.Web.Services
{
    public sealed class ImageStore
    {
        private readonly IWebHostEnvironment _env;

        public ImageStore(IWebHostEnvironment env) => _env = env;

        public IEnumerable<string> GetSources()
        {
            string fullPath = Path.GetFullPath("img", _env.WebRootPath);
            var directory = new DirectoryInfo(fullPath);
            return GetSources(directory);
        }

        private IEnumerable<string> GetSources(DirectoryInfo directory)
        {
            FileInfo[] files = directory.GetFiles("*.svg", SearchOption.AllDirectories);
            foreach (FileInfo file in files)
            {
                string relativePath = file.FullName.Remove(0, _env.WebRootPath.Length);
                relativePath = "~" + relativePath.Replace('\\', '/');
                yield return relativePath;
            }
        }
    }
}
