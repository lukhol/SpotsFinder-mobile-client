using Newtonsoft.Json;
using System;

namespace SpotFinder.Redux.StateModels
{
    public class User
    {
        [JsonProperty("id")]
        public long Id { get; private set; }

        [JsonProperty("firstname")]
        public string Firstname { get; private set; }

        [JsonProperty("lastname")]
        public string Lastname { get; private set; }

        [JsonProperty("email")]
        public string Email { get; private set; }

        [JsonProperty("facebookId")]
        public string FacebookId { get; private set; }

        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }

        [Obsolete]
        public User() { }

        public User(
            long id, 
            string firstname,
            string lastname, 
            string email, 
            string facebookId, 
            string accessToken, 
            string refreshToken
            )
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            FacebookId = facebookId;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
