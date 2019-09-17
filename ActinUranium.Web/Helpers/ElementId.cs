using System;
using System.Text.RegularExpressions;

namespace ActinUranium.Web.Helpers
{
    public static class ElementId
    {
        // See: https://stackoverflow.com/a/42026123
        public static string Next()
        {
            var guid = Guid.NewGuid();
            byte[] bytes = guid.ToByteArray();
            string base64 = Convert.ToBase64String(bytes);

            // The conversion is unidirectional, as we are loosing some data, by design.
            return Regex.Replace(base64, "[/+=]", string.Empty);
        }
    }
}
