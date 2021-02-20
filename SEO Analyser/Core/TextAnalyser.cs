using SEO_Analyser.Abstraction;
using SEO_Analyser.Utilities;
using System.Collections.Generic;

namespace SEO_Analyser.Core
{
    public class TextAnalyser : BaseAnalyser
    {
        private Dictionary<string, int> stopwordDictionary;

        public TextAnalyser(string input, string stopWords, bool isFilterStopWords, bool isCalculateOccuranceInText, bool isCalculateOccuranceInMetaTag, bool isCalculateExternalLink)
        {
            Input = input;
            StopWords = stopWords;
            IsFilterStopWords = isFilterStopWords;
            IsCalculateOccuranceInText = isCalculateOccuranceInText;
            IsCalculateOccuranceInMetaTag = isCalculateOccuranceInMetaTag;
            IsCalculateExternalLink = isCalculateExternalLink;
        }


        /// <summary>
        /// Calculates number of occurrences of each word in a english text.
        /// </summary>
        /// <returns>Dictionary of each word.</returns>
        public override Dictionary<string, int> CalculateOccuranceInText()
        {
            if (IsFilterStopWords && stopwordDictionary == null)
                stopwordDictionary = CoreUtil.ProcessStopWords(StopWords);

            return CoreUtil.ProcessInput(Input, stopwordDictionary);
        }

        /// <summary>
        /// Calculates number of occurrences of each word listed in keywords Meta Tag.
        /// </summary>
        /// <returns>A Empty Dictionary as english text do not consists of keywords meta tags.</returns>
        public override Dictionary<string, int> CalculateOccuranceInMetaTag() => new Dictionary<string, int>();

        /// <summary>
        /// Calculates number of external links in a english text.
        /// </summary>
        /// <returns>Dictionary of external link.</returns>
        public override Dictionary<string, int> CalculateExternalLink() => CoreUtil.ProcessExternalLink(Input);
    }
}