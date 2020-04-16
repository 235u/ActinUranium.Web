using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;

namespace ActinUranium.Web.TagHelpers
{
    public sealed class SvgSymbolTagHelper : SvgTagHelper
    {
        private const string IdAttributeName = "id";

        [ActivatorUtilitiesConstructor]
        public SvgSymbolTagHelper(IWebHostEnvironment hostingEnvironment)
            : base(hostingEnvironment)
        {
        }

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Reinitialize("svg", TagMode.StartTagAndEndTag);
            output.Attributes.SetAttribute("class", "svg-symbol");

            XElement svg = GetSvg();
            XElement symbol = BuildSymbol(svg);
            string content = GetContent(symbol);

            output.Content.SetHtmlContent(content);
        }

        private XElement BuildSymbol(XElement svg)
        {
            XNamespace ns = svg.GetDefaultNamespace();
            var elementName = XName.Get("symbol", ns.NamespaceName);
            var idAttribute = new XAttribute(IdAttributeName, Id);
            return new XElement(elementName, idAttribute, svg.Attributes(), svg.Elements());
        }
    }
}
