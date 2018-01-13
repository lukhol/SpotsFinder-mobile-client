using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SpotFinder.Redux.StateModels;
using SpotFinder.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.DataServices
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;
        private readonly IURLRepository urlRepository;
        private readonly JsonSerializer camelCaseJsonSerializer;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public UserService(HttpClient httpClient, IURLRepository urlRepository, JsonSerializer camelCaseJsonSerializer)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.urlRepository = urlRepository ?? throw new ArgumentNullException(nameof(urlRepository));
            this.camelCaseJsonSerializer = camelCaseJsonSerializer ?? throw new ArgumentNullException(nameof(camelCaseJsonSerializer));

            this.jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
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
            catch (Exception exception)
            {
                //TO DO: log...
                throw exception;
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
            catch (Exception exception)
            {
                //TO DO: log...
                throw new Exception("Exception in UserService.GetTokensAsync(...)", exception);
            }
        }

        [Obsolete("Not implemented yet!")]
        public async Task<User> RegisterAsync(User userToRegister)
        {
            try
            {
                var uri = new Uri(urlRepository.RegisterUserUri());
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                var response = await httpClient.PostAsync(uri, CreateStringContent(userToRegister));

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(responseContent, jsonSerializerSettings);

                return user;
            }
            catch(Exception ex)
            {
                //TODO: Log...
                throw new Exception("Could not register user :(", ex);
            }
        }

        public async Task<User> LoginOrRegisterAndLoginExternalUserAsync(User userToRegister, string accessToken)
        {
            try
            {
                var uri = new Uri(urlRepository.PostExternalUserUri(accessToken));
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                var jObjectUser = JObject.FromObject(userToRegister, camelCaseJsonSerializer);
                var stringContent = new StringContent(jObjectUser.ToString(), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, stringContent);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(responseContent, jsonSerializerSettings);

                return user;
            }
            catch (Exception exception)
            {
                //TODO: Log...
                throw exception;
            }
        }

        private StringContent CreateStringContent<T>(T objectValue)
        {
            var jObject = JObject.FromObject(objectValue, camelCaseJsonSerializer);
            var stringContent = new StringContent(jObject.ToString(), Encoding.UTF8, "application/json");
            return stringContent;
        }
    }
}
