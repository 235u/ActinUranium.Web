using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActinUranium.Web.Helpers
{
    [TestClass]
    public class LoremIpsumTests
    {
        [TestMethod]
        public void GetWords()
        {
            LoremIpsum.GetWords(9);
        }

        [TestMethod]
        public void NextHeading()
        {
            for (int minWordCount = 1; minWordCount <= 4; minWordCount++)
            {
                string heading = LoremIpsum.NextHeading(minWordCount, maxWordCount: minWordCount);
            }
        }

        [TestMethod]
        public void NextHeading_SingleLetter()
        {
            string text = LoremIpsum.NextHeading(1, 1);
            Assert.AreEqual(1, text.Length);
        }
    }
}
