using ActinUranium.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ActinUranium.Web.Models
{
    public sealed class Headline : IRelease
    {
        internal const int SlugMaxLength = 128;

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

        public Image GetPrimaryImage() =>
            HeadlineImages.OrderBy(hi => hi.DisplayOrder).FirstOrDefault()?.Image;

        public IReadOnlyCollection<Image> GetImages() =>
            HeadlineImages.OrderBy(hi => hi.DisplayOrder).Select(hi => hi.Image).ToList();
    }
}
