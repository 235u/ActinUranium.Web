using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ActinUranium.Web.TagHelpers
{
    public sealed class SvgUseTagHelper : TagHelper
    {
        public string SymbolId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Reinitialize("svg", TagMode.StartTagAndEndTag);
            ProcessAttributes(context, output);
            string content = $"<use xlink:href=\"#{SymbolId}\" />";
            output.Content.SetHtmlContent(content);
        }

        private static void ProcessAttributes(TagHelperContext context, TagHelperOutput output)
        {
            IEnumerable<TagHelperAttribute> attributes = context.AllAttributes
                .Where(a => !a.Name.Equals("symbol-id", StringComparison.OrdinalIgnoreCase));

            foreach (TagHelperAttribute attribute in attributes)
            {
                output.Attributes.SetAttribute(attribute);
            }
        }
    }
}
