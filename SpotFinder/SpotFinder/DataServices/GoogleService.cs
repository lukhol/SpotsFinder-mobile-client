using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotFinder.Models.DTO;
using SpotFinder.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpotFinder.DataServices
{
    public class GoogleService : IExternalUserService<SimpleGoogleUserDTO>
    {
        private const string CLIENT_ID = "293230980926-0k7h1g248tto4a5t98p13a5gkbvt3436.apps.googleusercontent.com";
        private const string CLIENT_SECRET = "c_Np6mfhLfLP499RIxkqGNuw";
        private const string REDIRECT_URI = "http://www.google.pl";
        private const string GRANT_TYPE = "authorization_code";
        private const string GOOGLE_QUERY_STRING = "https://www.googleapis.com/oauth2/v4/token";

        private readonly string googleQueryString;
        private readonly URLRepository urlRepository;
        private readonly JsonSerializer camelCaseJsonSerializer;

        public GoogleService(URLRepository urlRepository, JsonSerializer camelCaseJsonSerializer)
        {
            this.urlRepository = urlRepository ?? throw new ArgumentNullException(nameof(urlRepository));
            this.camelCaseJsonSerializer = camelCaseJsonSerializer ?? throw new ArgumentNullException(nameof(camelCaseJsonSerializer));
        }

        public async Task<string> GetAccessTokenAsync(string code)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    Uri uri = new Uri(GOOGLE_QUERY_STRING);

                    var keyValues = new KeyValuePair<string, string>[]
                    {
                        new KeyValuePair<string, string>("code", code),
                        new KeyValuePair<string, string>("client_id", CLIENT_ID),
                        new KeyValuePair<string, string>("client_secret", CLIENT_SECRET),
                        new KeyValuePair<string, string>("redirect_uri", REDIRECT_URI),
                        new KeyValuePair<string, string>("grant_type", GRANT_TYPE)
                    };

                    FormUrlEncodedContent httpContent = new FormUrlEncodedContent(keyValues);
                    HttpResponseMessage response = await httpClient.PostAsync(uri, httpContent);

                    response.EnsureSuccessStatusCode();

                    string responseJson = await response.Content.ReadAsStringAsync();
                    JObject jObjectResponse = JObject.Parse(responseJson);
                    string accessToken = (string)jObjectResponse["access_token"];

                    return accessToken;
                }
            }
            catch (Exception ex)
            {
                //TODO: Log...
                throw new Exception("Error in GoogleService.GetAccessTokenAsync(code).", ex);
            }
        }

        public async Task<SimpleGoogleUserDTO> GetExternalUserInfoAsync(string accessToken)
        {
            try
            {
                Uri uri = new Uri(urlRepository.GetGoogleUserInfoUri(accessToken));

                using (var httpClient = new HttpClient())
                {
                    var userJson = await httpClient.GetStringAsync(uri);
                    dynamic data = JsonConvert.DeserializeObject(userJson, typeof(object));

                    string smallAvatarUrl = data.@image.@url;

                    return new SimpleGoogleUserDTO
                    {
                        Id = data.@id,
                        Email = null,
                        Firstname = data.@name.@givenName,
                        Lastname = data.@name.@familyName,
                        AvatarUrl = ConvertSmallToLargeAvatarUrl(smallAvatarUrl)
                    };
                }
            }
            catch (Exception exception)
            {
                //TODO: Log...
                throw new Exception("Exception in GoogleService.GetExternalUserInfoAsync(accessToken).", exception);
            }
        }

        private string ConvertSmallToLargeAvatarUrl(string smallAvatarUrl)
        {
            if (!smallAvatarUrl.Contains("?sz="))
                return smallAvatarUrl;

            var largeAvatarUrl = smallAvatarUrl.Replace("?sz=50", "?sz=200");
            return largeAvatarUrl;
        }
    }
}
