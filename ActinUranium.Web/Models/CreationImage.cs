using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    }
}
