using Newtonsoft.Json;

namespace SpotFinder.Models.DTO
{
    public class SimpleGoogleUserDTO
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
    }
}
