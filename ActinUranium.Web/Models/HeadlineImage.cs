using ActinUranium.Web.Helpers;
using ActinUranium.Web.Services;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

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
    }
}
