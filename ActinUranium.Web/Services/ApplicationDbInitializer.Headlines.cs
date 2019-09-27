using ActinUranium.Web.Helpers;
using ActinUranium.Web.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        private static IReadOnlyCollection<Tag> CreateTags(int count)
        {
            var tags = new List<Tag>();

            string[] tagNames = CreateUniqueSet(count, () => LoremIpsum.NextWord(minLength: 7));
            foreach (string name in tagNames)
            {
                Tag tag = CreateTag(name);
                tags.Add(tag);
            }

            return tags;
        }

        private static Tag CreateTag(string name)
        {
            return new Tag
            {
                Slug = name.Slugify(),
                Name = name
            };
        }

        private static WeightedLottery<Tag> CreateTagLottery(IReadOnlyCollection<Tag> tags, int totalWeight)
        {
            var tagLottery = new WeightedLottery<Tag>();            

            foreach (Tag tag in tags)
            {
                int tagWeight = totalWeight / tags.Count;
                if (tag == tags.First())
                {
                    // total weight: 11, tag count: 2, first tag's weight: 6, second tag's weight: 5
                    tagWeight += totalWeight % tags.Count;
                }

                tagLottery.Add(tag, tagWeight);
            }

            return tagLottery;
        }

        private static Headline CreateHeadline(HeadlineInfo headline)
        {
            return new Headline
            {
                Slug = headline.Title.Slugify(),
                Title = headline.Title,
                Lead = LoremIpsum.NextParagraph(minSentenceCount: 1, maxSentenceCount: 2),
                Text = LoremIpsum.NextParagraph(minSentenceCount: 2, maxSentenceCount: 4),
                ReleaseDate = ActinUraniumInfo.NextDate(),
                Author = headline.Author,
                Tag = headline.Tag,
                HeadlineImages = headline.Images
            };
        }

        private void SeedHeadlines()
        {            
            IEnumerable<HeadlineInfo> headlineInfos = GetHeadlineInfos();           
            foreach (var info in headlineInfos)
            {
                Headline headline = CreateHeadline(info);
                _dbContext.Add(headline);
            }

            _dbContext.SaveChanges();
        }

        private IEnumerable<HeadlineInfo> GetHeadlineInfos()
        {
            var headlineInfos = new List<HeadlineInfo>();

            Author author = CreateAuthor();
            DirectoryInfo[] imageDirectories = GetDirectories("img/headlines");

            int illustratedHeadlineCount = imageDirectories.Length;
            int nonIllustratedHeadlineCount = illustratedHeadlineCount / 2;
            int totalHeadlineCount = illustratedHeadlineCount + nonIllustratedHeadlineCount;

            IReadOnlyCollection<Tag> tags = CreateTags(count: 2);
            WeightedLottery<Tag> tagLottery = CreateTagLottery(tags, totalWeight: illustratedHeadlineCount);
            string[] titles = CreateUniqueSet(
                totalHeadlineCount, () => LoremIpsum.NextHeading(minWordCount: 2, maxWordCount: 8));            

            for (int headlineIndex = 0; headlineIndex < illustratedHeadlineCount; headlineIndex++)
            {
                DirectoryInfo directory = imageDirectories[headlineIndex];
                var headlineInfo = new HeadlineInfo
                {
                    Images = GetHeadlineImages(directory),
                    Tag = tagLottery.Pull(),
                    Title = titles[headlineIndex],
                    Author = author
                };

                headlineInfos.Add(headlineInfo);
            }

            tagLottery = CreateTagLottery(tags, totalWeight: nonIllustratedHeadlineCount);
            for (int headlineIndex = illustratedHeadlineCount; headlineIndex < totalHeadlineCount; headlineIndex++)
            {
                var headlineInfo = new HeadlineInfo
                {
                    Images = new List<HeadlineImage>(),
                    Tag = tagLottery.Pull(),
                    Title = titles[headlineIndex],
                    Author = author
                };

                headlineInfos.Add(headlineInfo);
            }

            return headlineInfos;
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

        private class HeadlineInfo
        {
            public List<HeadlineImage> Images { get; set; }

            public Tag Tag { get; set; }

            public string Title { get; set; }            

            public Author Author { get; set; }
        }
    }
}
