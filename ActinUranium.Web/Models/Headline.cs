using ActinUranium.Web.Extensions;
using ActinUranium.Web.Helpers;
using ActinUranium.Web.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ActinUranium.Web.Models
{
    public sealed class Headline : IRelease
    {
        public const int SlugMaxLength = 128;
        public const int Count = 20;

        [Key]
        [MaxLength(SlugMaxLength)]
        public string Slug { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(SlugMaxLength)]
        [Display(Name = "Überschrift")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(256)]
        [Display(Name = "Vorspann")]
        public string Lead { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(1024)]
        public string Text { get; set; }

        [Column(TypeName = TransactSqlTypeNames.Date)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = FormatStrings.ShortDate)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [ForeignKey(nameof(Author))]
        [MaxLength(Author.SlugMaxLength)]
        public string AuthorSlug { get; set; }

        public Author Author { get; set; }

        [Required]
        [ForeignKey(nameof(Tag))]
        [MaxLength(Author.SlugMaxLength)]
        public string TagSlug { get; set; }

        public Tag Tag { get; set; }

        public List<HeadlineImage> HeadlineImages { get; set; } = new List<HeadlineImage>();

        internal static void Seed(ApplicationDbContext dbContext)
        {
            var authors = dbContext.Authors.ToList();
            var authorLottery = new Lottery<Author>(authors);

            var tags = dbContext.Tags.ToList();
            var tagLottery = new Lottery<Tag>(tags);

            for (int count = 0; count < Count; count++)
            {
                Headline headline = Create(dbContext);

                Tag headlineTag = tagLottery.Next();
                headline.TagSlug = headlineTag.Slug;

                Author headlineAuthor = authorLottery.Next();
                headline.AuthorSlug = headlineAuthor.Slug;

                dbContext.Headlines.Add(headline);
            }

            dbContext.SaveChanges();
        }

        private static Headline Create(ApplicationDbContext dbContext)
        {
            string slug;
            string title;

            while (true)
            {
                title = LoremIpsum.NextHeading(2, 8);
                slug = title.Slugify();
                bool isUnique = dbContext.Headlines.Find(slug) == null;
                if (isUnique)
                {
                    break;
                }
            }

            return new Headline()
            {
                Slug = slug,
                Title = title,
                Lead = LoremIpsum.NextParagraph(1, 2),
                Text = LoremIpsum.NextParagraph(2, 4),
                ReleaseDate = ActinUraniumInfo.NextDate()
            };
        }

        public Image GetPrimaryImage()
        {
            return HeadlineImages.OrderBy(hi => hi.DisplayOrder).FirstOrDefault()?.Image;
        }

        public IReadOnlyCollection<Image> GetImages()
        {
            return HeadlineImages.OrderBy(hi => hi.DisplayOrder).Select(hi => hi.Image).ToList();
        }
    }
}
