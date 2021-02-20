using SEO_Analyser.Abstraction;
using System.Collections.Generic;

namespace SEO_Analyser.Core
{
    public class CoreProcessor
    {
        public void AnalyzeData(BaseAnalyser analyser, out Dictionary<string, int> occuranceInTextDictionary, out Dictionary<string, int> occuranceInMetaTagDictionary, out Dictionary<string, int> ExternalLinkDictionary)
        {
            occuranceInTextDictionary = null;
            occuranceInMetaTagDictionary = null;
            ExternalLinkDictionary = null;

            if (analyser.IsCalculateOccuranceInText)
                occuranceInTextDictionary = analyser.CalculateOccuranceInText();

            if (analyser.IsCalculateOccuranceInMetaTag)
                occuranceInMetaTagDictionary = analyser.CalculateOccuranceInMetaTag();

            if (analyser.IsCalculateExternalLink)
                ExternalLinkDictionary = analyser.CalculateExternalLink();
        }
    }
}

