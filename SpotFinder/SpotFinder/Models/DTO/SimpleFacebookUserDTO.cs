using Newtonsoft.Json;

namespace SpotFinder.Models.DTO
{
    public class SimpleFacebookUserDTO
    {
        public string Id { get; set; }

        [JsonProperty("first_name")]
        public string Firstname { get; set; }

        [JsonProperty("last_name")]
        public string Lastname { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
