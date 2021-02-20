using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using SEO_Analyser.Constants;

namespace SEO_Analyser.Utilities
{
    public static class CoreUtil
    {
        private static readonly char[] delimiterChars = { ' ', ',', '.', ':', '"', '“', '”', '\t', '\r', '\n', '/', '(', ')', '[', ']', '<', '>' };

        /// <summary>
        /// Split the stop-words by whitespace and insert into stopwordDictionary.
        /// </summary>
        /// <param name="stopWords">The string of stop-words.</param>
        /// <returns>Empty Dictionary if the stopWords parameter is empty string ("") or consists only of white-space character; otherwise, Dictionary of stop-words.</returns>
        public static Dictionary<string, int> ProcessStopWords(string stopWords)
        {
            Dictionary<string, int> stopwordDictionary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

            if (!string.IsNullOrWhiteSpace(stopWords))
            {
                string[] stopWordArray = stopWords.Split(' ');

                foreach (var stopword in stopWordArray)
                {
                    StringBuilder sb = new StringBuilder();
                    string cleanWord;

                    foreach (var ch in stopword)
                    {
                        if (char.IsLetter(ch) || ch.Equals('\''))
                        {
                            sb.Append(ch);
                        }
                    }

                    cleanWord = sb.ToString();

                    if (!string.IsNullOrWhiteSpace(cleanWord))
                    {
                        if (!stopwordDictionary.ContainsKey(cleanWord))
                        {
                            stopwordDictionary.Add(cleanWord, 1);
                        }
                        else
                            stopwordDictionary[cleanWord]++;
                    }
                }
            }

            return stopwordDictionary;
        }

        /// <summary>
        /// Remove URL text and split the input text by delimiter character. 
        /// Filter out the stop-words and insert into inputDictionary.
        /// </summary>
        /// <param name="input">The string of input text.</param>
        /// <param name="stopwordDictionary">Dictionary of stop-words to be filtered out</param>
        /// <returns>Dictionary of each word.</returns>
        public static Dictionary<string, int> ProcessInput(string input, Dictionary<string, int> stopwordDictionary)
        {
            Dictionary<string, int> inputDictionary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

            //filter out the link
            input = Regex.Replace(input, Constant.GET_EXTERNAL_LINK_REGEX, "");

            string[] inputArray = input.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in inputArray)
            {
                StringBuilder sb = new StringBuilder();
                string cleanWord;

                foreach (var ch in word)
                {
                    if (char.IsLetter(ch) || ch.Equals('\''))
                    {
                        sb.Append(ch);
                    }
                }

                cleanWord = sb.ToString();

                if (!string.IsNullOrWhiteSpace(cleanWord))
                {
                    if (!inputDictionary.ContainsKey(cleanWord))
                        inputDictionary.Add(cleanWord, 1);
                    else
                        inputDictionary[cleanWord]++;
                }
            }

            //filter out stop-words if the stopwordDictionary is not null
            if (stopwordDictionary != null)
            {
                foreach (var sw in stopwordDictionary)
                {
                    if (inputDictionary.ContainsKey(sw.Key))
                        inputDictionary.Remove(sw.Key);
                }
            }

            return inputDictionary;
        }

        /// <summary>
        /// Get the value of content atrribute from SEO related meta tags.
        /// </summary>
        /// <param name="metaTagCollection">A collection of meta tag node.</param>
        /// <param name="allWordDictionary">A dictionary of each word.</param>
        /// <param name="stopwordDictionary">A dictionary of stop-word.</param>
        /// <returns>Dictionary of each word listed in SEO related meta tags.</returns>
        public static Dictionary<string, int> ProcessMetaTag(HtmlNodeCollection metaTagCollection, Dictionary<string, int> stopwordDictionary, Dictionary<string, int> allWordDictionary)
        {
            Dictionary<string, int> metaTagDictionary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            StringBuilder sb = new StringBuilder();
            if (metaTagCollection == null)
                return metaTagDictionary;

            foreach (var mt in metaTagCollection)
            {
                if (mt.Attributes[Constant.NAME] != null)
                {
                    // check only title, keywords and description meta tags
                    if (mt.Attributes[Constant.NAME].Value.ToLower() == Constant.KEYWORDS
                        || mt.Attributes[Constant.NAME].Value.ToLower() == Constant.DESCRIPTION
                        || mt.Attributes[Constant.NAME].Value.ToLower() == Constant.TITLE)
                    {
                        sb.AppendLine(mt.Attributes[Constant.CONTENT].Value);
                    }
                }
            }

            string clean = sb.ToString();
            metaTagDictionary = ProcessInput(clean, stopwordDictionary);

            // if allWordDictionary is empty, display directly the metaTagDictionary with word count in tag
            // otherwise, display the total number of occurence of each word listed in Meta Tag for that page
            if (allWordDictionary != null && metaTagDictionary != null)
            {
                foreach (var key in metaTagDictionary.Keys.ToList())
                {
                    if (allWordDictionary.ContainsKey(key))
                        metaTagDictionary[key] = allWordDictionary[key];
                    else
                        metaTagDictionary[key] = 0;
                }
            }
            else
            {
                foreach (var key in metaTagDictionary.Keys.ToList())
                {
                    metaTagDictionary[key] = 0;
                }
            }

            return metaTagDictionary;
        }

        /// <summary>
        /// Get external links from input text using Regular Expression.
        /// </summary>
        /// <param name="input">The string of input text.</param>
        /// <returns>Dictionary of external link.</returns>
        public static Dictionary<string, int> ProcessExternalLink(string input)
        {
            Dictionary<string, int> externalLinkDictionary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            MatchCollection mc = Regex.Matches(input, Constant.GET_EXTERNAL_LINK_REGEX);
            if (mc.Count > 0)
            {
                foreach (Match match in mc)
                {
                    if (!externalLinkDictionary.ContainsKey(match.Value))
                    {
                        externalLinkDictionary.Add(match.Value, 1);
                    }
                    else
                        externalLinkDictionary[match.Value]++;
                }
            }

            return externalLinkDictionary;
        }

        /// <summary>
        /// Get external links from hyperlink tag.
        /// </summary>
        /// <param name="hrefNodeCollection">A collection of hyperlink node.</param>
        /// <param name="baseURL">baseURL of target internet source.</param>
        /// <returns>Dictionary of external link.</returns>
        public static Dictionary<string, int> ProcessExternalLink(HtmlNodeCollection hrefNodeCollection, string baseURL)
        {
            Dictionary<string, int> externalLinkDictionary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

            if (hrefNodeCollection == null)
                return null;

            foreach (var node in hrefNodeCollection)
            {
                var href = node.Attributes["href"].Value;
                var uri = new Uri(href, UriKind.RelativeOrAbsolute);
                string realUri;
                if (!uri.IsAbsoluteUri)
                    uri = new Uri(new Uri(baseURL), uri);

                realUri = uri.ToString();
                if (!externalLinkDictionary.ContainsKey(realUri))
                {
                    externalLinkDictionary.Add(realUri, 1);
                }
                else
                    externalLinkDictionary[realUri]++;
            }

            return externalLinkDictionary;
        }
    }
}