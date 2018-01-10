using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SpotFinder.Redux.StateModels;
using SpotFinder.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpotFinder.DataServices
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;
        private readonly IURLRepository urlRepository;
        private readonly JsonSerializer camelCaseJsonSerializer;

        public UserService(HttpClient httpClient, IURLRepository urlRepository, JsonSerializer camelCaseJsonSerializer)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.urlRepository = urlRepository ?? throw new ArgumentNullException(nameof(urlRepository));
            this.camelCaseJsonSerializer = camelCaseJsonSerializer ?? throw new ArgumentNullException(nameof(camelCaseJsonSerializer));
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            try
            {
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                var uri = new Uri(urlRepository.LoginUri(email, password));
                var response = await httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jsonSerializerSettings = new JsonSerializerSettings();
                    jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    var user = JsonConvert.DeserializeObject<User>(responseContent, jsonSerializerSettings);

                    return user;
                }
                else
                {
                    throw new Exception("Not authorized.");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Tuple<string, string>> GetTokensAsync(string username, string password)
        {
            var keyValues = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            };

            try
            {
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                Uri uri = new Uri(urlRepository.TokensUri());
                FormUrlEncodedContent httpContent = new FormUrlEncodedContent(keyValues);
                HttpResponseMessage response = await httpClient.PostAsync(uri, httpContent);

                response.EnsureSuccessStatusCode();

                string responseJson = await response.Content.ReadAsStringAsync();
                JObject jObjectResponse = JObject.Parse(responseJson);
                string accessToken = (string)jObjectResponse["access_token"];
                string refresToken = (string)jObjectResponse["refresh_token"];

                return new Tuple<string, string>(accessToken, refresToken);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<User> RegisterAsync(User userToRegister)
        {
            throw new NotImplementedException();
        }

        public async Task<User> RegisterUsingFacebookAsyc(User userToRegister, string accessToken)
        {
            throw new NotImplementedException();
        }
    }
}
