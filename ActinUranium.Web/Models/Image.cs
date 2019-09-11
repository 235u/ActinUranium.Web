using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ActinUranium.Web.Models
{
    [Description("Bild")]
    public class Image
    {
        public const int SourceMaxLength = 128;

        /// <summary>
        /// Gets or sets the web root relative path.
        /// </summary>
        /// <see cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/index#web-root"/>
        /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files#serve-static-files"/>
        [Key]
        [MaxLength(SourceMaxLength)]
        [DataType(DataType.Url)]
        public string Source { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(128)]
        [Display(Name = "Alternativer Text", Prompt = "Ein vollständiger Satz")]
        public string AlternativeText { get; set; }

        [Display(Name = "Titel", Description = "Der Titel wird üblicherweise in Form eines Tooltips dargestellt.")]
        [MaxLength(64)]
        public string Title { get; set; }
    }
}
