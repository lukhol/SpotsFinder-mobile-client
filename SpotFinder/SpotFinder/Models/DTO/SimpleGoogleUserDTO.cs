using Newtonsoft.Json;

namespace SpotFinder.Models.DTO
{
    public class SimpleGoogleUserDTO
    {
        [JsonProperty("id")]
        public string Id { get; private set; }
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public string Email { get; private set; }
    }
}
