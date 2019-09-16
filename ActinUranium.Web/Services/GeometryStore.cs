using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;

namespace ActinUranium.Web.Services
{
    public sealed class GeometryStore
    {
        private IHostingEnvironment _env;

        public GeometryStore(IHostingEnvironment env)
        {
            _env = env;
        }

        public IEnumerable<string> GetSources()
        {
            string fullPath = Path.GetFullPath("img/geometry", _env.WebRootPath);
            var directory = new DirectoryInfo(fullPath);
            return GetSources(directory);
        }

        private IEnumerable<string> GetSources(DirectoryInfo directory)
        {
            FileInfo[] files = directory.GetFiles("*.svg", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                string relativePath = file.FullName.Remove(0, _env.WebRootPath.Length);
                relativePath = "~" + relativePath.Replace('\\', '/');
                yield return relativePath;
            }
        }
    }
}
