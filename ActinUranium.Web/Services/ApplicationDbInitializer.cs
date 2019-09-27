using ActinUranium.Web.Helpers;
using ActinUranium.Web.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ActinUranium.Web.Services
{
    public sealed partial class ApplicationDbInitializer
    {
        private readonly IHostingEnvironment _env;
        private readonly ApplicationDbContext _dbContext;

        public ApplicationDbInitializer(IHostingEnvironment env, ApplicationDbContext dbContext)
        {
            _env = env;
            _dbContext = dbContext;
        }

        public void SeedData()
        {
            SeedCustomers();
            SeedCreations();
            SeedHeadlines();
        }

        private static string[] CreateUniqueStringSet(int elementCount, Func<string> createFunction)
        {
            var set = new HashSet<string>();

            for (int actualCount = 0; actualCount < elementCount; actualCount++)
            {
                bool added = false;
                while (!added)
                {
                    string value = createFunction();
                    added = set.Add(value);
                }
            }

            return set.ToArray();
        }

        private static Image CreateImage(string source)
        {
            return new Image
            {
                Source = source,
                AlternativeText = LoremIpsum.NextSentence()
            };
        }

        private DirectoryInfo[] GetDirectories(string relativePath)
        {
            string fullPath = Path.GetFullPath(relativePath, _env.WebRootPath);
            var directory = new DirectoryInfo(fullPath);
            return directory.GetDirectories();
        }

        private IEnumerable<string> GetImageSources(DirectoryInfo directory)
        {
            FileInfo[] files = directory.GetFiles("*.svg");
            foreach (FileInfo file in files)
            {
                string relativePath = file.FullName.Remove(0, _env.WebRootPath.Length);
                relativePath = "~" + relativePath.Replace('\\', '/');
                yield return relativePath;
            }
        }        
    }
}
