namespace SEO_Analyser.Constants
{
    public class Constant
    {
        //meta tag attributes
        public const string NAME = "name";
        public const string KEYWORDS = "keywords";
        public const string TITLE = "title";
        public const string DESCRIPTION = "description";
        public const string CONTENT = "content";

        //Regex
        public const string GET_EXTERNAL_LINK_REGEX = @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)";
        public const string GET_VALIDATE_HTML_TAG_REGEX = @"<\/?[a-z|!][\s\S]*";

        //Error Message
        public const string EMPTY_INPUT = "** Input field cannot be blank.";
        public const string INVALID_URI_FORMAT = "** Invalid URL format.";
        public const string WEB_REQUEST_ERROR_MESSAGE = "** Web Request Error, Requested URL Returned Error : ";

    }
}