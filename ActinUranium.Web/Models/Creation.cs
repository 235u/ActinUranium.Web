using ActinUranium.Web.Helpers;
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
        [Display(Name = SharedResources.Strategy)]
        public string Strategy { get; set; }

        [Required]
        [MaxLength(256)]
        [Display(Name = SharedResources.Execution)]
        public string Execution { get; set; }

        [Display(Name = "Bilder")]
        public List<CreationImage> CreationImages { get; set; }

        public Image GetPrimaryImage() =>
            CreationImages.OrderBy(ci => ci.DisplayOrder).First().Image;

        public IReadOnlyCollection<Image> GetImages() =>
            CreationImages.OrderBy(ci => ci.DisplayOrder).Select(ci => ci.Image).ToList();
    }
}
