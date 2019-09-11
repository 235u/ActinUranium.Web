using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace ActinUranium.Web.Services
{
    /// <summary>
    /// Customizes token replacement in route templates.
    /// </summary>
    /// <remarks>Converts <c>TermsAndConditions</c> to <c>terms-and-conditions</c>.</remarks>
    /// <see href="https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-2.2#token-replacement-in-route-templates-controller-action-area"/>
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        private const string Pattern = "([a-z])([A-Z])";
        private const string Replacement = "$1-$2";

        public string TransformOutbound(object value)
        {
            if (value == null)
            {
                return null;
            }

            var input = value.ToString();
            var result = Regex.Replace(input, Pattern, Replacement).ToLower();
            return result;
        }
    }
}
