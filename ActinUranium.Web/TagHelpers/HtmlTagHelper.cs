using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ActinUranium.Web.TagHelpers
{
    public class HtmlTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the current <see cref="ViewContext"/>.
        /// </summary>
        /// <remarks>
        /// Decoration with the <see cref="ViewContextAttribute"/> is required for the injection of the current
        /// <see cref="ViewContext" /> at tag helper's construction.
        /// </remarks>
        /// <seealso href="https://github.com/aspnet/Mvc/issues/4744#issuecomment-221572659"/>
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string lang = GetLanguageTag();
            if (!string.IsNullOrEmpty(lang))
            {
                output.Attributes.SetAttribute(nameof(lang), lang);
            }
        }

        private string GetLanguageTag()
        {
            // For language tag format, see: https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/lang
            return ViewContext.HttpContext
                .Features
                .Get<IRequestCultureFeature>()?
                .RequestCulture
                .UICulture
                .IetfLanguageTag;
        }
    }
}
