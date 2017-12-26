using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SpotFinder.Core
{
    public static class GlobalSettings
    {
        public const string PostSpotUrl = "http://81.2.255.46:8080/places";
        public const string GetPlaceByCriteriaUrl = "http://81.2.255.46:8080/places/searches";
        public const string GetAllUrl = "http://81.2.255.46:8080/places";
        public const string GetByIdUrl = "http://81.2.255.46:8080/places/";

        public static JsonSerializer GetCamelCaseSerializer()
        {
            var jsonSerializer = new JsonSerializer();
            jsonSerializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return jsonSerializer;
        }
    }
}
