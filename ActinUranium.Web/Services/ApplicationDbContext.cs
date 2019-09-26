using ActinUranium.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreating(modelBuilder.Entity<Author>());
            OnModelCreating(modelBuilder.Entity<CreationImage>());
            OnModelCreating(modelBuilder.Entity<Customer>());
            OnModelCreating(modelBuilder.Entity<HeadlineImage>());
            OnModelCreating(modelBuilder.Entity<Tag>());
        }

        private static void OnModelCreating(EntityTypeBuilder<Author> entity)
        {
            entity.HasIndex(author => author.FullName)
                .IsUnique();

            entity.HasIndex(author => author.Email)
                .IsUnique();
        }

        private static void OnModelCreating(EntityTypeBuilder<CreationImage> entity)
        {
            entity.HasIndex(creationImage => new { creationImage.CreationSlug, creationImage.DisplayOrder })
                .IsUnique();

            entity.HasOne(creationImage => creationImage.Creation)
                .WithMany(creation => creation.CreationImages)
                .HasForeignKey(creationImage => creationImage.CreationSlug);
        }

        private static void OnModelCreating(EntityTypeBuilder<Customer> entity)
        {
            entity.HasIndex(customer => customer.Name)
                .IsUnique();
        }

        private static void OnModelCreating(EntityTypeBuilder<HeadlineImage> entity)
        {
            entity.HasIndex(headlineImage => new { headlineImage.HeadlineSlug, headlineImage.DisplayOrder })
                .IsUnique();

            entity.HasOne(headlineImage => headlineImage.Headline)
                .WithMany(headline => headline.HeadlineImages)
                .HasForeignKey(headlineImage => headlineImage.HeadlineSlug);
        }

        private static void OnModelCreating(EntityTypeBuilder<Tag> entity)
        {
            entity.HasIndex(tag => tag.Name)
                .IsUnique();
        }
    }
}
