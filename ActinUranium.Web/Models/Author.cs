using System.ComponentModel.DataAnnotations;

namespace ActinUranium.Web.Models
{
    public sealed class Author
    {
        internal const int SlugMaxLength = 32;

        [Key]
        [MaxLength(SlugMaxLength)]
        public string Slug { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(SlugMaxLength)]
        public string FullName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(64)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
