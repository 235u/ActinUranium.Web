using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ActinUranium.Web.Extensions;
using ActinUranium.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace ActinUranium.Web.Models
{
    public class Author
    {
        public const int SlugMaxLength = 32;

        [Key]
        [MaxLength(SlugMaxLength)]
        public string Slug { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(SlugMaxLength)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(64)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        internal static void OnModelCreating(EntityTypeBuilder<Author> typeBuilder)
        {
            typeBuilder.HasIndex(a => a.Name).IsUnique();
            typeBuilder.HasIndex(a => a.Email).IsUnique();
        }

        internal static void Seed(ApplicationDbContext dbContext)
        {
            const string FullName = "Legion von Gadara";
            var author = new Author()
            {
                Slug = FullName.Slugify(),
                Name = FullName,
                Email = "legion@235u.net"
            };

            dbContext.Authors.Add(author);
            dbContext.SaveChanges();
        }
    }
}
