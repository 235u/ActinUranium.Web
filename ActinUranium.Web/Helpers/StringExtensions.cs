using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ActinUranium.Web.Helpers
{
    internal static class StringExtensions
    {
        public static bool ContainsWord(this string text, string word)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            string pattern = $@"\b{word}\b";
            return Regex.IsMatch(text, pattern);
        }

        public static string Limit(this string text, int maxLength)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (text.Length > maxLength)
            {
                int startIndex = maxLength - 1;
                const char Ellipsis = '…';
                return text.Remove(startIndex) + Ellipsis;
            }

            return text;
        }

        public static string[] SplitIntoSentences(this string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            // See: https://stackoverflow.com/a/4957295
            const string Pattern = @"(?<=[\.!\?])\s+";
            return Regex.Split(text, Pattern);
        }

        public static string[] SplitIntoWords(this string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            char[] separator = { ' ', ',', '.', '?' };
            return text.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .Select(word => word.ToLowerInvariant())
                .ToArray();
        }

        public static string ToSentenceCase(this string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (text.Length == 0)
            {
                string message = $"'{nameof(text)}' cannot be empty.";
                throw new ArgumentException(message, nameof(text));
            }

            // See: https://stackoverflow.com/a/27073919
            char[] charArray = text.ToCharArray();
            charArray[0] = char.ToUpperInvariant(charArray[0]);
            return new string(charArray);
        }

        public static string Slugify(this string text) => 
            text.Trim().CollapseAjacentWhitespace().Replace(' ', '-').ToLowerInvariant();

        // replacing all whitespace characters like 'NO-BREAK SPACE' with common 'SPACE' characters
        public static string CollapseAjacentWhitespace(this string text) => Regex.Replace(text, @"\s+", " ");
    }
}
