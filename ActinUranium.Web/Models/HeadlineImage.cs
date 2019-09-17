using ActinUranium.Web.Helpers;
using ActinUranium.Web.Services;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActinUranium.Web.Models
{
    public sealed class HeadlineImage
    {
        [Key]
        [MaxLength(Image.SourceMaxLength)]
        public string ImageSource { get; set; }

        [ForeignKey(nameof(ImageSource))]
        public Image Image { get; set; }

        [MaxLength(Headline.SlugMaxLength)]
        public string HeadlineSlug { get; set; }

        [ForeignKey(nameof(HeadlineSlug))]
        public Headline Headline { get; set; }

        [Display(Name = "Anzeigereihenfolge")]
        public byte DisplayOrder { get; set; } = 1;

        internal static void OnModelCreating(EntityTypeBuilder<HeadlineImage> typeBuilder)
        {
            typeBuilder.HasIndex(ci => new { ci.HeadlineSlug, ci.DisplayOrder })
                .IsUnique();

            typeBuilder.HasOne(ci => ci.Headline)
                .WithMany(c => c.HeadlineImages)
                .HasForeignKey(ci => ci.HeadlineSlug);
        }

        internal static void Seed(ApplicationDbContext dbContext)
        {
            var imageLottery = new WeightedLottery<bool>
            {
                { true, 16 },
                { false, 4 }
            };

            var imageNumberLottery = new Int32Lottery(1, 17);

            foreach (Headline headline in dbContext.Headlines)
            {
                bool doCreateImage = imageLottery.Pull();
                if (doCreateImage)
                {
                    int imageNumber = imageNumberLottery.Pull();
                    string fileName = string.Format("{0:00}.svg", imageNumber);
                    Image image = CreateImage(fileName);
                    dbContext.Images.Add(image);

                    var headlineImage = new HeadlineImage()
                    {
                        ImageSource = image.Source,
                        HeadlineSlug = headline.Slug
                    };

                    dbContext.HeadlineImages.Add(headlineImage);
                }
            }

            dbContext.SaveChanges();
        }

        private static Image CreateImage(string fileName)
        {
            return new Image()
            {
                Source = $"~/img/headlines/{fileName}",
                AlternativeText = LoremIpsum.NextSentence(),
            };
        }
    }
}
