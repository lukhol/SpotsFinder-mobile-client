using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.Core
{
    public static class GlobalSettings
    {
        //public static string GetPlaceByCriteriaUrl = "http://demo5878347.mockable.io/spots/criteria";
        //public static string PostSpotUrl = "http://demo5878347.mockable.io/post";

        public static string PostSpotUrl = "http://8ef07ba5.ngrok.io/places";
        public static string GetPlaceByCriteriaUrl = "http://8ef07ba5.ngrok.io/places/searches";
        public static string GetAllUrl = "http://8ef07ba5.ngrok.io/places";

        public static JsonSerializer GetCamelCaseSerializer()
        {
            var jsonSerializer = new JsonSerializer();
            jsonSerializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return jsonSerializer;
        }
    }
}
