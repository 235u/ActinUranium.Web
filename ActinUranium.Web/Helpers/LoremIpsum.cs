using ActinUranium.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActinUranium.Web.Helpers
{
    public static class LoremIpsum
    {
        /// <summary>
        /// Represents the sections 1.10.32-33 of "De finibus bonorum et malorum", written by Cicero in 45 BC.
        /// </summary>
        private const string OriginalText = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur? At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae. Itaque earum rerum hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat.";

        private static readonly string[] Sentences = OriginalText.SplitIntoSentences();
        private static readonly string[] Words = OriginalText.SplitIntoWords();
        private static readonly Random Random = new Random();

        public static string[] GetWords(int length)
        {
            return Words.Where(w => w.Length == length).OrderBy(w => w).ToArray();
        }

        public static string[] GetWords(char firstCharacter)
        {
            return Words.Where(w => w.StartsWith(firstCharacter)).OrderBy(w => w).ToArray();
        }

        public static string NextHeading(int minWordCount, int maxWordCount)
        {
            string heading = NextWordGroup(minWordCount, maxWordCount);
            return heading.ToSentenceCase();
        }

        public static string NextParagraph(int minSentenceCount, int maxSentenceCount)
        {
            string paragraph = string.Empty;

            int actualSentenceCount = 0;
            int targetSentenceCount = GetSoftCount(minSentenceCount, maxSentenceCount);

            while (actualSentenceCount < targetSentenceCount)
            {
                string sentence = NextSentence();
                if (!paragraph.Contains(sentence))
                {
                    paragraph = Concat(paragraph, sentence);
                    actualSentenceCount += 1;
                }
            }

            return paragraph;
        }

        public static string NextWord(int minLength)
        {
            string[] subset = Words.Where(w => w.Length >= minLength).ToArray();
            int index = Random.Next(subset.Length);
            return subset[index];
        }

        public static string NextWordGroup(int minWordCount, int maxWordCount)
        {
            if (minWordCount < 1)
            {
                string message = $"'{nameof(minWordCount)}' cannot be smaller than '1'.";
                throw new ArgumentOutOfRangeException(nameof(minWordCount), message);
            }

            if (minWordCount > maxWordCount)
            {
                string message = $"'{nameof(minWordCount)}' cannot be greater than '{nameof(maxWordCount)}'.";
                throw new ArgumentOutOfRangeException(nameof(minWordCount), message);
            }

            var wordGroup = string.Empty;

            int actualWordCount = 0;
            int targetWordCount = GetSoftCount(minWordCount, maxWordCount);

            while (actualWordCount < targetWordCount)
            {
                string word = NextWord();
                if (!wordGroup.ContainsWord(word))
                {
                    wordGroup = Concat(wordGroup, word);
                    actualWordCount += 1;
                }
            }

            return wordGroup;
        }

        public static string NextKeywords()
        {
            var keywords = NextWordGroup(3, 5).Split(' ');
            return string.Join(", ", keywords);
        }

        
        public static string NextDescription()
        {
            var wordLottery = new Lottery<string>(Words);
            var keywords = new List<string>();            

            // See: https://blog.spotibo.com/meta-description-length/
            int softCharLimit = GetSoftCount(64, 128);
            int actualCharCount = 0;

            while (actualCharCount < softCharLimit)
            {
                string word = wordLottery.Pull();
                if (actualCharCount == 0)
                {
                    word = word.ToSentenceCase();
                }

                // Consider spaces between words and the dot at the end of the final sentence.
                actualCharCount += word.Length + 1;
                keywords.Add(word);
            }

            string description = string.Join(" ", keywords);
            return description + ".";
        }

        private static int GetSoftCount(int minCount, int maxCount)
        {
            int exclusiveMaxCount = maxCount + 1;
            return Random.Next(minCount, exclusiveMaxCount);
        }

        private static string NextWord()
        {
            int index = Random.Next(Words.Length);
            return Words[index];
        }

        public static string NextSentence()
        {
            int index = Random.Next(Sentences.Length);
            return Sentences[index];
        }

        public static string NextSentence(int minWordCount, int maxWordCount)
        {
            return NextSentence(minWordCount, maxWordCount, '.');
        }

        public static string NextSentence(int minWordCount, int maxWordCount, char endMark)
        {
            string words = NextWordGroup(minWordCount, maxWordCount);
            return words.ToSentenceCase() + endMark;
        }

        private static string Concat(string firstString, string secondString)
        {
            if (firstString.Length > 0)
            {
                firstString += " ";
            }

            return firstString + secondString;
        }
    }
}
