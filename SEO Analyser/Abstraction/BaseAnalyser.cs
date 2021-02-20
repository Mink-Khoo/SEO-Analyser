using System.Collections.Generic;

namespace SEO_Analyser.Abstraction
{
    public abstract class BaseAnalyser
    {
        public string Input { get; set; }
        public string StopWords { get; set; }
        public bool IsURL { get; set; }
        public bool IsFilterStopWords { get; set; }
        public bool IsCalculateOccuranceInText { get; set; }
        public bool IsCalculateOccuranceInMetaTag { get; set; }
        public bool IsCalculateExternalLink { get; set; }
        public abstract Dictionary<string, int> CalculateOccuranceInText();
        public abstract Dictionary<string, int> CalculateOccuranceInMetaTag();
        public abstract Dictionary<string, int> CalculateExternalLink();

    }
}