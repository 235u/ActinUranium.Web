using ActinUranium.Web.Helpers;
using ActinUranium.Web.Models;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ActinUranium.Web.Services
{
    public sealed class ApplicationDbInitializer
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
            Customer.Seed(_dbContext);
            Creation.Seed(_dbContext);

            SeedCreationImages();

            Author.Seed(_dbContext);
            Tag.Seed(_dbContext);
            Headline.Seed(_dbContext);

            SeedHeadlineImages();
        }

        private static Image CreateImage(string source)
        {
            return new Image
            {
                Source = source,
                AlternativeText = LoremIpsum.NextSentence()
            };
        }

        private void SeedCreationImages()
        {
            DirectoryInfo[] directories = GetDirectories("img/creations");
            for (int count = 0; count < directories.Length; count++)
            {
                Creation creation = _dbContext.Creations.Skip(count).First();

                IEnumerable<string> imageSources = GetSources(directories[count]);
                foreach (string source in imageSources)
                {
                    Image image = CreateImage(source);
                    _dbContext.Images.Add(image);

                    var creationImage = new CreationImage
                    {
                        ImageSource = image.Source,
                        CreationSlug = creation.Slug
                    };

                    _dbContext.CreationImages.Add(creationImage);
                }
            }

            _dbContext.SaveChanges();
        }

        private void SeedHeadlineImages()
        {
            var headlines = _dbContext.Headlines.ToList();
            var headlineLottery = new Lottery<Headline>(headlines);

            DirectoryInfo[] directories = GetDirectories("img/headlines");
            for (int count = 0; count < directories.Length; count++)
            {
                Headline headline = headlineLottery.Pull();

                IEnumerable<string> imageSources = GetSources(directories[count]);
                foreach (string source in imageSources)
                {
                    Image image = CreateImage(source);
                    _dbContext.Images.Add(image);

                    var headlineImage = new HeadlineImage
                    {
                        ImageSource = image.Source,
                        HeadlineSlug = headline.Slug
                    };

                    _dbContext.HeadlineImages.Add(headlineImage);
                }
            }

            _dbContext.SaveChanges();
        }

        private DirectoryInfo[] GetDirectories(string relativePath)
        {
            string fullPath = Path.GetFullPath(relativePath, _env.WebRootPath);
            var directory = new DirectoryInfo(fullPath);
            return directory.GetDirectories();
        }

        private IEnumerable<string> GetSources(DirectoryInfo directory)
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
