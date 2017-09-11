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
        public static string PostSpotUrl = "http://85.255.10.140:8080/places";
        public static string GetPlaceByCriteriaUrl = "http://85.255.10.140:8080/places/searches";
        public static string GetAllUrl = "http://85.255.10.140:8080/places";
        public static string GetByIdUrl = "http://85.255.10.140:8080/places/";

        public static JsonSerializer GetCamelCaseSerializer()
        {
            var jsonSerializer = new JsonSerializer();
            jsonSerializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return jsonSerializer;
        }
    }
}
