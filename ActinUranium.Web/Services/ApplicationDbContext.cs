using ActinUranium.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ActinUranium.Web.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Creation> Creations { get; set; }

        public DbSet<CreationImage> CreationImages { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Headline> Headlines { get; set; }

        public DbSet<HeadlineImage> HeadlineImages { get; set; }

        public DbSet<Image> Images { get; set; }

        /// <summary>
        /// Populates the database with an initial set of data.
        /// </summary>
        public void Seed()
        {
            Customer.Seed(this);
            Creation.Seed(this);
            CreationImage.Seed(this);
            Author.Seed(this);
            Tag.Seed(this);
            Headline.Seed(this);
            HeadlineImage.Seed(this);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Author.OnModelCreating(modelBuilder.Entity<Author>());
            CreationImage.OnModelCreating(modelBuilder.Entity<CreationImage>());
            HeadlineImage.OnModelCreating(modelBuilder.Entity<HeadlineImage>());
        }
    }
}
