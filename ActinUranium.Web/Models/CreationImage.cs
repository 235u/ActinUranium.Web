using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ActinUranium.Web.Helpers;
using ActinUranium.Web.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ActinUranium.Web.Models
{
    public sealed class CreationImage
    {
        [Key]
        [MaxLength(Image.SourceMaxLength)]
        public string ImageSource { get; set; }

        [ForeignKey(nameof(ImageSource))]
        public Image Image { get; set; }

        [MaxLength(Creation.SlugMaxLength)]
        public string CreationSlug { get; set; }

        [ForeignKey(nameof(CreationSlug))]
        public Creation Creation { get; set; }

        [Display(Name = "Anzeigereihenfolge")]
        public byte DisplayOrder { get; set; } = 1;

        internal static void OnModelCreating(EntityTypeBuilder<CreationImage> typeBuilder)
        {
            typeBuilder.HasIndex(ci => new { ci.CreationSlug, ci.DisplayOrder })
                .IsUnique();

            typeBuilder.HasOne(ci => ci.Creation)
                .WithMany(c => c.CreationImages)
                .HasForeignKey(ci => ci.CreationSlug);
        }

        internal static void Seed(ApplicationDbContext dbContext)
        {
            for (int count = 1; count <= 32; count++)
            {
                string fileName = string.Format("{0:00}.svg", count);
                Image image = CreateImage(fileName);
                dbContext.Images.Add(image);

                Creation creation = dbContext.Creations.Skip(count - 1).First();
                var creationImage = new CreationImage()
                {
                    ImageSource = image.Source,
                    CreationSlug = creation.Slug
                };

                dbContext.CreationImages.Add(creationImage);
            }

            dbContext.SaveChanges();
        }

        private static Image CreateImage(string fileName)
        {
            return new Image()
            {
                Source = $"~/img/creations/{fileName}",                
                AlternativeText = LoremIpsum.NextSentence()
            };
        }
    }
}
