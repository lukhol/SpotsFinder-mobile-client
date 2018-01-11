using System.Globalization;

namespace SpotFinder.Repositories
{
    public class URLRepository : IURLRepository
    {
        //private const string BASE_URL = "http://81.2.255.46:8080";
        private const string BASE_URL = "http://abaacdcd.ngrok.io";
        public string PostErrorUri => string.Format("{0}/{1}", BASE_URL, "errors");
        public string GetPlacesUri => string.Format("{0}/{1}", BASE_URL, "places");
        public string PostPlaceUri => string.Format("{0}/{1}", BASE_URL, "places");
        public string GetPlaceByCriteriaUri => string.Format("{0}/{1}", BASE_URL, "places/searches");

        public string API_KEY => "spotfinder:spotfinderSecret";

        public string GetPlaceByIdUri(int id)
        {
            return string.Format("{0}/{1}/{2}", BASE_URL, "places", id);
        }

        public string LoginUri(string email, string password)
        {
            return string.Format("{0}/{1}?email={2}&password={3}", BASE_URL,"user/login", email, password);
        }

        public string TokensUri()
        {
            return string.Format("{0}/{1}", BASE_URL, "oauth/token");
        }

        public string GetFacebookInfoUri(string accessToken)
        {
            return string.Format("https://graph.facebook.com/v2.7/me/?fields=id,email,first_name,last_name&access_token={0}", accessToken);
        }

        public string PostExternalUserUri(string accessToken)
        {
            return string.Format("{0}/{1}?externalAccessToken={2}", BASE_URL, "user/login/external", accessToken);
        }

        public string PostWrongPlaceReportUri => string.Format("{0}/{1}", BASE_URL, "places/report");
    }
}
