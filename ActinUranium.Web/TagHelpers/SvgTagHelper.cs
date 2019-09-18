using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace ActinUranium.Web.TagHelpers
{
    public class SvgTagHelper : TagHelper
    {
        private const string SrcAttributeName = "src";

        [ActivatorUtilitiesConstructor]
        public SvgTagHelper(IHostingEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
        }

        [HtmlAttributeName(SrcAttributeName)]
        public string Src { get; set; }

        private IHostingEnvironment HostingEnvironment { get; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!string.IsNullOrEmpty(Src))
            {
                output.TagName = string.Empty;

                XElement tree = GetSvg();
                string content = GetContent(tree);

                output.Content.SetHtmlContent(content);
            }
        }

        protected static string GetContent(XElement svg)
        {
            SaveOptions options = SaveOptions.DisableFormatting | SaveOptions.OmitDuplicateNamespaces;
            string content = svg.ToString(options);
            return RemoveNamespaceAttributes(content);
        }

        protected XElement GetSvg()
        {
            string fullPath = GetFullPath();
            string svg = File.ReadAllText(fullPath);
            return XElement.Parse(svg);
        }

        private string GetFullPath()
        {
            const string TildeSlash = "~/";
            string relativePath = Src.Remove(0, TildeSlash.Length);
            return Path.GetFullPath(relativePath, basePath: HostingEnvironment.WebRootPath);
        }

        // See: https://stackoverflow.com/a/18468348
        private static string RemoveNamespaceAttributes(string content)
        {
            var comparisonType = StringComparison.OrdinalIgnoreCase;                        
            return content.Replace("xmlns=\"http://www.w3.org/2000/svg\"", string.Empty, comparisonType)
                .Replace("xmlns:xlink=\"http://www.w3.org/1999/xlink\"", string.Empty, comparisonType);
        }
    }
}
