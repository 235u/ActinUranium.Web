using ActinUranium.Web.Helpers;
using ActinUranium.Web.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ActinUranium.Web.Services
{
    public sealed partial class ApplicationDbInitializer
    {
        private static Creation CreateCreation(string title, Customer customer, List<CreationImage> images)
        {
            return new Creation
            {
                Slug = title.Slugify(),
                Title = title,
                ReleaseDate = ActinUraniumInfo.NextDate(),
                Mission = LoremIpsum.NextParagraph(2, 3),
                Strategy = LoremIpsum.NextParagraph(1, 3),
                Execution = LoremIpsum.NextParagraph(1, 3),
                Customer = customer,
                CreationImages = images
            };
        }

        private void SeedCreations()
        {
            Lottery<Customer> customerLottery = GetCustomerLottery();
            DirectoryInfo[] imageDirectories = GetDirectories("img/creations");
            int creationCount = imageDirectories.Length;
            string[] creationTitles = CreateUniqueSet(
                creationCount, () => LoremIpsum.NextHeading(minWordCount: 2, maxWordCount: 3));            

            for (int creationIndex = 0; creationIndex < creationCount; creationIndex++)
            {
                string title = creationTitles[creationIndex];
                Customer customer = customerLottery.Next();
                DirectoryInfo imageDirectory = imageDirectories[creationIndex];
                List<CreationImage> images = GetCreationImages(imageDirectory);
                Creation creation = CreateCreation(title, customer, images);
                _dbContext.Creations.Add(creation);
            }

            _dbContext.SaveChanges();
        }

        private Lottery<Customer> GetCustomerLottery()
        {
            var customers = _dbContext.Customers.ToList();
            return new Lottery<Customer>(customers);
        }

        private List<CreationImage> GetCreationImages(DirectoryInfo directory)
        {
            var creationImages = new List<CreationImage>();

            IEnumerable<string> imageSources = GetImageSources(directory);
            foreach (string source in imageSources)
            {
                var creationImage = new CreationImage
                {
                    Image = CreateImage(source),
                };

                creationImages.Add(creationImage);
            }

            return creationImages;
        }
    }
}
