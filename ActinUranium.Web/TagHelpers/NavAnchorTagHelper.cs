using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace ActinUranium.Web.TagHelpers
{
    [HtmlTargetElement("a", ParentTag = "nav")]
    public class NavAnchorTagHelper : AnchorTagHelper
    {
        private const string ClassAttributeName = "class";

        // assuming further css class names in front
        private const string ActiveClassName = " btn-active";

        public NavAnchorTagHelper(IHtmlGenerator generator)
            : base(generator)
        {
        }

        // Seems to run after processing the base tag helper, so no 'base.Process(context, output)' required.
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string classAttributeValue = string.Empty;
            if (output.Attributes.TryGetAttribute(ClassAttributeName, out TagHelperAttribute attribute))
            {
                classAttributeValue = ((HtmlString)attribute.Value).Value;
            }

            var comparisonType = StringComparison.OrdinalIgnoreCase;
            if (IsActive() && !classAttributeValue.Contains(ActiveClassName, comparisonType))
            {
                classAttributeValue += ActiveClassName;
            }
            else
            {
                classAttributeValue = classAttributeValue.Replace(ActiveClassName, string.Empty, comparisonType);
            }

            output.Attributes.SetAttribute(ClassAttributeName, classAttributeValue);
        }

#pragma warning disable IDE0019 // Use pattern matching
        private bool IsActive()
        {
            string currentController = ViewContext.RouteData.Values["controller"] as string;
            string currentAction = ViewContext.RouteData.Values["action"] as string;
            return (currentController != null) && (currentController == Controller) &&
                ((currentAction == Action) || (currentAction == "Details"));
        }
#pragma warning restore IDE0019 // Use pattern matching
    }
}
