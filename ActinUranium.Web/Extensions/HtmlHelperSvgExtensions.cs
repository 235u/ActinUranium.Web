using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace ActinUranium.Web.Extensions
{
    public static class HtmlHelperSvgExtensions
    {
        private const string TildeSlash = "~/";

        public static IHtmlContent Svg(this IHtmlHelper helper, string virtualPath)
        {
            string svg = ReadAllText(helper, virtualPath);
            return helper.Raw(svg);
        }

        public static IHtmlContent SvgSymbol(this IHtmlHelper helper, string virtualPath, string id)
        {
            string svg = ReadAllText(helper, virtualPath);

            var tree = XElement.Parse(svg);

            XNamespace ns = tree.GetDefaultNamespace();
            var elementName = XName.Get("symbol", ns.NamespaceName);

            var idAttribute = new XAttribute("id", id);

            XAttribute viewBoxAttribute = tree.Attribute("viewBox");
            viewBoxAttribute.Remove();

            var symbol = new XElement(elementName, idAttribute, viewBoxAttribute);

            IEnumerable<XElement> children = tree.Elements();
            symbol.Add(children);

            tree.ReplaceNodes(symbol);
            tree.SetAttributeValue("style", "display: none");

            SaveOptions options = SaveOptions.DisableFormatting | SaveOptions.OmitDuplicateNamespaces;
            svg = tree.ToString(options);

            return helper.Raw(svg);
        }

        public static IHtmlContent SvgUse(this IHtmlHelper helper, string symbolId)
        {
            string svg = $"<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\"><use xlink:href=\"#{symbolId}\" /></svg>";
            return helper.Raw(svg);
        }

        public static IHtmlContent SvgUse(this IHtmlHelper helper, string symbolId, string className)
        {
            string svg = $"<svg class=\"{className}\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\"><use xlink:href=\"#{symbolId}\" /></svg>";
            return helper.Raw(svg);
        }

        private static string ReadAllText(IHtmlHelper helper, string virtualPath)
        {
            if (virtualPath == null)
            {
                throw new ArgumentNullException(nameof(virtualPath));
            }

            if (!IsVirtualPath(virtualPath))
            {
                const string Message = "Path is not virtual (i.e. is not beginning with '~/').";
                throw new ArgumentException(Message, nameof(virtualPath));
            }

            string fullPath = GetFullPath(helper, virtualPath);
            return File.ReadAllText(fullPath);
        }

        private static string GetFullPath(IHtmlHelper helper, string virtualPath)
        {
            string basePath = GetWebRootPath(helper);
            string path = GetRelativePath(virtualPath);
            return Path.GetFullPath(path, basePath);
        }

        private static bool IsVirtualPath(string path)
        {
            return path.StartsWith(TildeSlash);
        }

        private static string GetRelativePath(string virtualPath)
        {
            return virtualPath.Remove(0, TildeSlash.Length);
        }

        private static string GetWebRootPath(IHtmlHelper helper)
        {
            var env = (IHostingEnvironment)helper.ViewContext.HttpContext.RequestServices.GetService(
                typeof(IHostingEnvironment));
            return env.WebRootPath;
        }
    }
}
