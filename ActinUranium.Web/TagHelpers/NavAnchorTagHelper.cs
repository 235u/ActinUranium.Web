using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

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

            if (IsActive() && !classAttributeValue.Contains(ActiveClassName))
            {
                classAttributeValue += ActiveClassName;
            }
            else
            {
                classAttributeValue = classAttributeValue.Replace(ActiveClassName, string.Empty);
            }

            output.Attributes.SetAttribute(ClassAttributeName, classAttributeValue);
        }

        private bool IsActive()
        {
            string currentController = ViewContext.RouteData.Values["controller"] as string;
            string currentAction = ViewContext.RouteData.Values["action"] as string;
            return (currentController != null) && (currentController == Controller) &&
                ((currentAction == Action) || (currentAction == "Details"));
        }
    }
}
