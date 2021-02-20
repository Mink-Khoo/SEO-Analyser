using NUnit.Framework;
using System;
using System.Collections.Generic;
using SEO_Analyser.Utilities;
using HtmlAgilityPack;

namespace SEOAnalyserTest
{
    [TestFixture]
    public class AnalyserTest
    {
        [TestCase("don't or and are the I'm")]
        [TestCase("#do!n't o$r a%n^d are the I'm666")]
        public void ProcessStopWord_WithEnglishText_ReturnDictionaryOfSixElements(string input)
        {
            //arrange
            var expected = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "don't", 1 },
                { "or", 1 },
                { "and", 1 },
                { "are", 1 },
                { "the", 1 },
                { "I'm", 1 }
            };

            //act
            var actual = CoreUtil.ProcessStopWords(input);

            //assert
            Assert.AreEqual(actual, expected);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("12345 !@#$%^&*()")]
        public void ProcessStopWord_TextWithNonLetterOrWhiteSpace_ReturnEmptyDictionary(string input)
        {
            //arrange
            var expected = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

            //act
            var actual = CoreUtil.ProcessStopWords(input);

            //assert
            Assert.AreEqual(actual, expected);
            Assert.IsEmpty(actual);
        }

        [TestCase("Hello, World.")]
        [TestCase("#Hello! World.")]
        public void ProcessInput_WithDifferentWordsAndPunctuations_ReturnDictionaryOfTwoKeyWithOneOcurranceEach(string input)
        {
            //arrange
            var stopWordDic = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            var expected = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
            {
                 { "Hello", 1 },
                 { "World", 1 }
             };

            //act
            var actual = CoreUtil.ProcessInput(input, stopWordDic);

            //assert
            Assert.AreEqual(actual, expected);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("12345 !@#$%^&*()")]
        public void ProcessInput_WithEmptyText_ReturnEmptyDictionary(string input)
        {
            //arrange
            var stopWordDic = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            var expected = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

            //act
            var actual = CoreUtil.ProcessInput(input, stopWordDic);

            //assert
            Assert.AreEqual(actual, expected);
            Assert.IsEmpty(actual);
        }

        [TestCase("Hello, World.")]
        [TestCase("#Hello! you're my World.")]
        public void ProcessInput_WithFilterOutStopWords_ReturnDictionaryOfTwoKeyWithOneOcurranceEach(string input)
        {
            //arrange
            var stopWordDic = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "you're", 1 },
                { "my", 1 },
            };

            var expected = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
            {
                 { "Hello", 1 },
                 { "World", 1 }
             };
            //act
            var actual = CoreUtil.ProcessInput(input, stopWordDic);

            //assert
            Assert.AreEqual(actual, expected);
        }

        [TestCase("Hello, World. https://google.com")]
        public void ProcessInput_WithValidUrlInText_ReturnDictionaryOfTwoKeyWithOneOcurranceEach(string input)
        {
            //arrange
            var stopWordDic = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            var expected = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
            {
                 { "Hello", 1 },
                 { "World", 1 }
             };

            //act
            var actual = CoreUtil.ProcessInput(input, stopWordDic);

            //assert
            Assert.AreEqual(actual, expected);
        }

        [TestCase("Hello, hello HELLO")]
        [TestCase("#Hello! hello. HELLO")]
        public void ProcessInput_WithIdenticalWordsDifferentLetterCase_ReturnDictionaryOfOneKeyWithThreeOcurrance(string input)
        {
            //arrange
            var stopWordDic = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            var expected = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
            {
                 { "Hello", 3 }
             };

            //act
            var actual = CoreUtil.ProcessInput(input, stopWordDic);

            //assert
            Assert.AreEqual(actual, expected);
        }

        [TestCase("<html></html>")]
        public void ProcessMetaTag_WithNoMetaTagsInHtml_WithEmptyWordDictionary_ReturnEmptyMetaTagDictionary(string html)
        {
            //arrange
            var stopwordDictionary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            var allWordDictionary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            var expected = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);


            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//meta");

            //act
            var actual = CoreUtil.ProcessMetaTag(nodes, stopwordDictionary, allWordDictionary);

            //assert
            Assert.AreEqual(actual, expected);
            Assert.IsEmpty(actual);
        }

        [TestCase("<meta name=\"title\" content=\"hello world, happy coding!\">" +
            "<meta name=\"keywords\" content=\"hello, world, coding\">" +
            "<meta name=\"description\" content=\"start coding today\">")]
        [TestCase(" <meta name=\"author\" content=\"Mink\">" +
            "<meta name=\"title\" content=\"hello world, happy coding!\">" +
            "<meta name=\"keywords\" content=\"hello, world, coding\">" +
            "<meta name=\"description\" content=\"start coding today\">")]
        public void ProcessMetaTag_WithDifferentMetaTagsInHtml_WithEmptyWordDictionary_ReturnDictionaryOfMetaTagWithZeorOccurence(string html)
        {
            //arrange
            var stopwordDictionary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            var allWordDictionary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            var expected = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "hello", 0 },
                { "world", 0 },
                { "happy", 0 },
                { "coding", 0 },
                { "start", 0 },
                { "today", 0 }
            };


            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//meta");

            //act
            var actual = CoreUtil.ProcessMetaTag(nodes, stopwordDictionary, allWordDictionary);

            //assert
            Assert.AreEqual(actual, expected);
        }

        [TestCase("<meta name=\"title\" content=\"hello world, happy coding!\">" +
            "<meta name=\"keywords\" content=\"hello, world, coding\">" +
            "<meta name=\"description\" content=\"start coding today\">")]
        public void ProcessMetaTag_WithWordsExistWithinMetaTag_ReturnDictionaryOfWordOccurenceListedInMetaTag(string html)
        {
            //arrange
            var stopwordDictionary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            var allWordDictionary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "happy", 5 },
                { "coding", 5 }
            };

            var expected = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "hello", 0 },
                { "world", 0 },
                { "happy", 5 },
                { "coding", 5 },
                { "start", 0 },
                { "today", 0 }
            };


            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//meta");

            //act
            var actual = CoreUtil.ProcessMetaTag(nodes, stopwordDictionary, allWordDictionary);

            //assert
            Assert.AreEqual(actual, expected);
        }

        [TestCase("<meta name=\"title\" content=\"hello world, happy coding!\">" +
            "<meta name=\"keywords\" content=\"hello, world, coding\">" +
            "<meta name=\"description\" content=\"start coding today\">")]
        public void ProcessMetaTag_WithWordsNotExistWithinMetaTag_ReturnDictionaryOfWordOccurenceListedInMetaTag(string html)
        {
            //arrange
            var stopwordDictionary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            var allWordDictionary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "happy", 5 },
                { "coding", 5 },
                { "Seo", 5 },
                { "Analyser", 5 },
                { "Text", 5 }
            };

            var expected = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "hello", 0 },
                { "world", 0 },
                { "happy", 5 },
                { "coding", 5 },
                { "start", 0 },
                { "today", 0 }
            };


            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//meta");

            //act
            var actual = CoreUtil.ProcessMetaTag(nodes, stopwordDictionary, allWordDictionary);

            //assert
            Assert.AreEqual(actual, expected);
        }

        [TestCase("")]
        [TestCase(" ")]
        public void ProcessExternalLink_WithEmptyText_ReturnDictionaryOfExternalLink(string input)
        {
            //arrange
            var expected = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);


            //act
            var actual = CoreUtil.ProcessExternalLink(input);

            //assert
            Assert.AreEqual(actual, expected);
            Assert.IsEmpty(actual);
        }

        [TestCase("hello, which is google url? http://www.google.com, https://www.google.com, www.google.com, google.com, https://www.google.com")]
        public void ProcessExternalLink_WithEnglishText_ReturnDictionaryOfExternalLink(string input)
        {
            //arrange
            var expected = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "http://www.google.com", 1 },
                { "https://www.google.com", 2 }
            };

            //act
            var actual = CoreUtil.ProcessExternalLink(input);

            //assert
            Assert.AreEqual(actual, expected);
        }

        [TestCase("<html><a target=\"_blank\" class=\"test\" href=\"https://www.google.com\">test</a>" +
            "<a target=\"_blank\" href=\"exercise.asp?filename=exercise_syntax1\">Start the Exercise</a></html>", "https://www.w3schools.com/cs/")]
        public void ProcessExternalLink_onURLAnalyserWithRelativeOrAbsoluteUrl_ReturnDictionaryOfExternalLink(string html, string baseUrl)
        {
            //arrange

            var expected = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "https://www.google.com/", 1 },
                { "https://www.w3schools.com/cs/exercise.asp?filename=exercise_syntax1", 1 }
            };

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//html//a");

            //act
            var actual = CoreUtil.ProcessExternalLink(nodes, baseUrl);

            //assert
            Assert.AreEqual(actual, expected);
        }
    }
}
