using ActinUranium.Web.Helpers;
using ActinUranium.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace ActinUranium.Web.Models
{
    public sealed class Tag
    {
        internal const int SlugMaxLength = 16;

        [Key]
        [MaxLength(SlugMaxLength)]
        public string Slug { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(SlugMaxLength)]
        [Display(Name = "Bezeichnung")]
        public string Name { get; set; }

        internal static void Seed(ApplicationDbContext dbContext)
        {
            for (int count = 0; count < 2; count++)
            {
                Tag tag = Create(dbContext);
                dbContext.Tags.Add(tag);
            }

            dbContext.SaveChanges();
        }

        private static Tag Create(ApplicationDbContext dbContext)
        {
            string slug;
            string name;

            while (true)
            {
                name = LoremIpsum.NextWord(7);
                slug = name.Slugify();
                bool isUnique = dbContext.Tags.Find(slug) == null;
                if (isUnique)
                {
                    break;
                }
            }

            return new Tag
            {
                Slug = slug,
                Name = name
            };
        }
    }
}
