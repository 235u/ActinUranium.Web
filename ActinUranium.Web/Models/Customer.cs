using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActinUranium.Web.Models
{
    [Description("Kunde")]
    public sealed class Customer
    {
        internal const int SlugMaxLength = 64;

        [Key]
        [MaxLength(SlugMaxLength)]
        public string Slug { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(SlugMaxLength)]
        [Display(Name = "Bezeichnung")]
        [DisplayFormat(HtmlEncode = false)]
        public string Name { get; set; }

        [Required]
        [MaxLength(Image.SourceMaxLength)]
        [DataType(DataType.Url)]
        public string LogoSource { get; set; }

        [ForeignKey(nameof(LogoSource))]
        public Image Logo { get; set; }

        public string Website =>
            "https://translate.google.com/#view=home&op=translate&sl=la&tl=de&text=" + Uri.EscapeDataString(Name);
    }
}
