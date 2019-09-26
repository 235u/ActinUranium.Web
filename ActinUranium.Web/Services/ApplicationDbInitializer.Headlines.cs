using ActinUranium.Web.Helpers;
using ActinUranium.Web.Models;
using System.Collections.Generic;
using System.IO;

namespace ActinUranium.Web.Services
{
    public sealed partial class ApplicationDbInitializer
    {
        private static Author CreateAuthor()
        {
            const string FullName = "Legion von Gadara";
            return new Author
            {
                Slug = FullName.Slugify(),
                FullName = FullName,
                Email = "legion@235u.net"
            };
        }

        private static WeightedLottery<Tag> CreateTagLottery(int headlineCount)
        {
            var tagLottery = new WeightedLottery<Tag>();

            const int TargetCount = 2;
            string[] tagNames = CreateUniqueSet(TargetCount, () => LoremIpsum.NextWord(minLength: 7));            

            for (int tagIndex = 0; tagIndex < TargetCount; tagIndex++)
            {
                string name = tagNames[tagIndex];
                Tag tag = CreateTag(name);
                int tagWeight = headlineCount / TargetCount;
                if (tagIndex == 0)
                {
                    // headline count: 11, first tag's weight: 6, second tag's weight: 5
                    tagWeight += headlineCount % TargetCount;
                }

                tagLottery.Add(tag, tagWeight);
            }

            return tagLottery;
        }

        private static Tag CreateTag(string name)
        {
            return new Tag
            {
                Slug = name.Slugify(),
                Name = name
            };
        }

        private static Headline CreateHeadline(string title, Author author, Tag tag, List<HeadlineImage> images)
        {
            return new Headline
            {
                Slug = title.Slugify(),
                Title = title,
                Lead = LoremIpsum.NextParagraph(minSentenceCount: 1, maxSentenceCount: 2),
                Text = LoremIpsum.NextParagraph(minSentenceCount: 2, maxSentenceCount: 4),
                ReleaseDate = ActinUraniumInfo.NextDate(),
                Author = author,
                Tag = tag,
                HeadlineImages = images
            };
        }

        private void SeedHeadlines()
        {
            Author author = CreateAuthor();            
            DirectoryInfo[] imageDirectories = GetDirectories("img/headlines");
            int headlineCount = imageDirectories.Length;
            WeightedLottery<Tag> tagLottery = CreateTagLottery(headlineCount);
            string[] headlineTitles = CreateUniqueSet(
                headlineCount, () => LoremIpsum.NextHeading(minWordCount: 2, maxWordCount: 8));            

            for (int headlineIndex = 0; headlineIndex < headlineCount; headlineIndex++)
            {
                string title = headlineTitles[headlineIndex];
                Tag tag = tagLottery.Pull();
                DirectoryInfo directory = imageDirectories[headlineIndex];
                List<HeadlineImage> images = GetHeadlineImages(directory);
                Headline headline = CreateHeadline(title, author, tag, images);
                _dbContext.Add(headline);
            }

            _dbContext.SaveChanges();
        }

        private List<HeadlineImage> GetHeadlineImages(DirectoryInfo directory)
        {
            var headlineImages = new List<HeadlineImage>();

            IEnumerable<string> imageSources = GetImageSources(directory);
            foreach (string source in imageSources)
            {                
                var headlineImage = new HeadlineImage
                {
                    Image = CreateImage(source),                    
                };

                headlineImages.Add(headlineImage);
            }

            return headlineImages;
        }
    }
}
