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

        [JsonProperty("googleId")]
        public string GoogleId { get; private set; }

        [JsonProperty("avatarUrl")]
        public string AvatarUrl { get; private set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; private set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; private set; }

        [JsonProperty("password")]
        public string Password { get; private set; }

        [Obsolete]
        public User() { }

        public User(
            long id, 
            string firstname,
            string lastname, 
            string email, 
            string facebookId, 
            string googleId,
            string avatarUrl,
            string accessToken, 
            string refreshToken,
            string password
            )
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            FacebookId = facebookId;
            GoogleId = googleId;
            AvatarUrl = avatarUrl;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Password = password;
        }
    }
}
