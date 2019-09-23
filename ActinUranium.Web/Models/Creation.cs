using ActinUranium.Web.Helpers;
using ActinUranium.Web.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ActinUranium.Web.Models
{
    [Description("Arbeit")]
    public sealed class Creation : IRelease
    {
        internal const int SlugMaxLength = 64;

        [Key]
        [MaxLength(SlugMaxLength)]
        public string Slug { get; set; }

        [MaxLength(SlugMaxLength)]
        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Required]
        [MaxLength(Customer.SlugMaxLength)]
        [Display(Name = "Kunde")]
        public string CustomerSlug { get; set; }

        [ForeignKey(nameof(CustomerSlug))]
        [Display(Name = "Kunde")]
        public Customer Customer { get; set; }

        [Column(TypeName = TransactSqlTypeNames.Date)]
        [DataType(DataType.Date)]
        [Display(Name = "Datum der Veröffentlichung")]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [MaxLength(64)]
        public string Mission { get; set; }

        [Required]
        [MaxLength(256)]
        [Display(Name = "Masterplan")]
        public string Strategy { get; set; }

        [Required]
        [MaxLength(256)]
        [Display(Name = "Maßnahmen")]
        public string Execution { get; set; }

        [Display(Name = "Bilder")]
        public List<CreationImage> CreationImages { get; set; }

        internal static void Seed(ApplicationDbContext dbContext)
        {
            var customers = dbContext.Customers.ToList();
            var customerLottery = new Lottery<Customer>(customers);

            for (int count = 0; count < 12; count++)
            {
                Creation creation = Create(dbContext);

                Customer creationCustomer = customerLottery.Next();
                creation.CustomerSlug = creationCustomer.Slug;

                dbContext.Creations.Add(creation);
            }

            dbContext.SaveChanges();
        }

        private static Creation Create(ApplicationDbContext dbContext)
        {
            string slug;
            string title;

            while (true)
            {
                title = LoremIpsum.NextHeading(2, 3);
                slug = title.Slugify();
                bool isUnique = dbContext.Creations.Find(slug) == null;
                if (isUnique)
                {
                    break;
                }
            }

            return new Creation
            {
                Slug = slug,
                Title = title,
                ReleaseDate = ActinUraniumInfo.NextDate(),
                Mission = LoremIpsum.NextParagraph(2, 3),
                Strategy = LoremIpsum.NextParagraph(1, 3),
                Execution = LoremIpsum.NextParagraph(1, 3)
            };
        }

        public Image GetPrimaryImage()
        {
            return CreationImages.OrderBy(ci => ci.DisplayOrder).First().Image;
        }

        public IReadOnlyCollection<Image> GetImages()
        {
            return CreationImages.OrderBy(ci => ci.DisplayOrder).Select(ci => ci.Image).ToList();
        }
    }
}
