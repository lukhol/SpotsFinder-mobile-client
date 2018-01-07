using System.Globalization;

namespace SpotFinder.Repositories
{
    public class URLRepository : IURLRepository
    {
        //private const string BASE_URL = "http://81.2.255.46:8080";
        private const string BASE_URL = "http://c0b63d02.ngrok.io";
        public string PostErrorUri => string.Format("{0}/{1}", BASE_URL, "errors");
        public string GetPlacesUri => string.Format("{0}/{1}", BASE_URL, "places");
        public string PostPlaceUri => string.Format("{0}/{1}", BASE_URL, "places");
        public string GetPlaceByCriteriaUri => string.Format("{0}/{1}", BASE_URL, "places/searches");

        public string API_KEY => "spot:finder";

        public string GetPlaceByIdUri(int id)
        {
            return string.Format("{0}{1}/{2}", BASE_URL, "places", id);
        }

        public string PostWrongPlaceReportUri => string.Format("{0}/{1}", BASE_URL, "places/report");
    }
}
