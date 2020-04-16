using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ActinUranium.Web.Helpers
{
    [TestClass]
    public class LoremIpsumTests
    {
        [TestMethod]
        public void GetWords()
        {
            const int WordLength = 9;
            string[] words = LoremIpsum.GetWords(WordLength);
            Assert.IsTrue(words.All(word => word.Length == WordLength));
        }

        [TestMethod]
        public void NextHeading()
        {
            for (int wordCount = 1; wordCount <= 4; wordCount++)
            {
                string heading = LoremIpsum.NextHeading(minWordCount: wordCount, maxWordCount: wordCount);
                Assert.AreEqual(wordCount, heading.SplitIntoWords().Length);
            }
        }
    }
}
