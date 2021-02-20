using SEO_Analyser.Abstraction;
using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using SEO_Analyser.Utilities;
using System.Net;
using SEO_Analyser.Constants;

namespace SEO_Analyser.Core
{
    public class URLAnalyser : BaseAnalyser
    {
        private Dictionary<string, int> stopwordDictionary;
        private Dictionary<string, int> allWordDictionary;
        HtmlDocument htmlPage;

        public URLAnalyser(string input, string stopWords, bool isFilterStopWords, bool isCalculateOccuranceInText, bool isCalculateOccuranceInMetaTag, bool isCalculateExternalLink)
        {
            Input = input;
            StopWords = stopWords;
            IsFilterStopWords = isFilterStopWords;
            IsCalculateOccuranceInText = isCalculateOccuranceInText;
            IsCalculateOccuranceInMetaTag = isCalculateOccuranceInMetaTag;
            IsCalculateExternalLink = isCalculateExternalLink;
        }

        /// <summary>
        /// Calculates number of occurrences of each word in a webpage.
        /// </summary>
        /// <returns>Dictionary of each word.</returns>
        public override Dictionary<string, int> CalculateOccuranceInText()
        {
            if (IsFilterStopWords && stopwordDictionary == null)
                stopwordDictionary = CoreUtil.ProcessStopWords(StopWords);

            if (htmlPage == null)
            {
                var htmlWeb = new HtmlWeb();
                var lastStatusCode = HttpStatusCode.OK;

                htmlWeb.PostResponse = (request, response) =>
                {
                    if (response != null)
                    {
                        lastStatusCode = response.StatusCode;
                    }
                };

                htmlPage = htmlWeb.Load(Input);

                if (lastStatusCode != HttpStatusCode.OK)
                    throw new Exception($"{Constant.WEB_REQUEST_ERROR_MESSAGE} {lastStatusCode}");
            }

            var bodyText = htmlPage.DocumentNode.SelectSingleNode("//body").InnerText;
            allWordDictionary = CoreUtil.ProcessInput(bodyText, stopwordDictionary);

            return allWordDictionary;
        }

        /// <summary>
        /// Calculates number of occurrences of each word listed in keywords Meta Tag.
        /// </summary>
        /// <returns>Dictionary of each word listed in keywords meta tags.</returns>
        public override Dictionary<string, int> CalculateOccuranceInMetaTag()
        {

            if (IsFilterStopWords && stopwordDictionary == null)
                stopwordDictionary = CoreUtil.ProcessStopWords(StopWords);

            if (htmlPage == null)
            {
                var htmlWeb = new HtmlWeb();
                var lastStatusCode = HttpStatusCode.OK;

                htmlWeb.PostResponse = (request, response) =>
                {
                    if (response != null)
                    {
                        lastStatusCode = response.StatusCode;
                    }
                };

                htmlPage = htmlWeb.Load(Input);

                if (lastStatusCode != HttpStatusCode.OK)
                    throw new Exception($"{Constant.WEB_REQUEST_ERROR_MESSAGE} {lastStatusCode}");
            }

            var metaTagCollection = htmlPage.DocumentNode.SelectNodes("//meta");
            return CoreUtil.ProcessMetaTag(metaTagCollection, stopwordDictionary, allWordDictionary);
        }

        /// <summary>
        /// Calculates number of external links in a webpage.
        /// </summary>
        /// <returns>Dictionary of external link.</returns>
        public override Dictionary<string, int> CalculateExternalLink()
        {
            if (htmlPage == null)
            {
                var htmlWeb = new HtmlWeb();
                var lastStatusCode = HttpStatusCode.OK;

                htmlWeb.PostResponse = (request, response) =>
                {
                    if (response != null)
                    {
                        lastStatusCode = response.StatusCode;
                    }
                };

                htmlPage = htmlWeb.Load(Input);

                if (lastStatusCode != HttpStatusCode.OK)
                    throw new Exception($"{Constant.WEB_REQUEST_ERROR_MESSAGE} {lastStatusCode}");
            }

            var hrefNodeCollection = htmlPage.DocumentNode.SelectNodes("//a[@href]");
            return CoreUtil.ProcessExternalLink(hrefNodeCollection, Input);
        }
    }
}