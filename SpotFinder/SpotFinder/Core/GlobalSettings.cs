using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SpotFinder.Core
{
    public static class GlobalSettings
    {
        public const string API_KEY = "spot:finder";
        public const string BASE_URL = "http://81.2.255.46:8080/";
        //public const string BASE_URL = "http://e0bdcc75.ngrok.io/";
        public const string POST_PLACE = BASE_URL  + "places";
        public const string GET_PLACES_BY_CRITERIA = BASE_URL + "places/searches";
        public const string GET_ALL = BASE_URL + "places";
        public const string GET_BY_ID = BASE_URL + "places/";

        public static JsonSerializer GetCamelCaseSerializer()
        {
            var jsonSerializer = new JsonSerializer();
            jsonSerializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return jsonSerializer;
        }
    }
}
