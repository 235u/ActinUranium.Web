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
    }
}
