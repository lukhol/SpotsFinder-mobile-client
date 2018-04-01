using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Redux;
using SpotFinder.Redux;
using SpotFinder.Redux.StateModels;
using SpotFinder.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.DataServices
{
    public class UserService : BaseService, IUserService
    {
        private readonly JsonSerializer camelCaseJsonSerializer;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public UserService(HttpClient httpClient, URLRepository urlRepository, JsonSerializer camelCaseJsonSerializer, IStore<ApplicationState> appStore) :
            base(httpClient, urlRepository, appStore)
        {
            this.camelCaseJsonSerializer = camelCaseJsonSerializer ?? throw new ArgumentNullException(nameof(camelCaseJsonSerializer));

            this.jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            try
            {
                SetBasicToken();
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
            SetBasicToken();
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
                return ExtractTokensFromResponse(responseJson);
            }
            catch (Exception exception)
            {
                //TO DO: log...
                throw new Exception("Exception in UserService.GetTokensAsync(...)", exception);
            }
        }

        public async Task<User> RegisterAsync(User userToRegister)
        {
            SetBasicToken();

            try
            {
                var uri = new Uri(urlRepository.RegisterUserUri());
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                var response = await httpClient.PostAsync(uri, CreateStringContent(userToRegister));
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.BadGateway)
                    throw new Exception("Server not responding.");

                if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                    throw new Exception(string.Join("\n", values.Select(m =>  m.Value).ToArray()));
                }

                response.EnsureSuccessStatusCode();

                var user = JsonConvert.DeserializeObject<User>(responseContent, jsonSerializerSettings);

                return user;
            }
            catch(Exception ex)
            {
                //TODO: Log...
                throw ex;
            }
        }

        public async Task<User> LoginOrRegisterAndLoginExternalUserAsync(User userToRegister, string accessToken)
        {
            SetBasicToken();

            try
            {
                var uri = new Uri(urlRepository.PostExternalUserUri(accessToken));
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                var response = await httpClient.PostAsync(uri, CreateStringContent(userToRegister));

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

        public async Task<string> SetAvatarAsync(long userId, Stream avatarStream)
        {
            SetBasicToken();

            if (avatarStream.Position != 0)
                avatarStream.Position = 0;

            try
            {
                Uri uri = new Uri(urlRepository.SetUserAvatarUri(userId.ToString()));
                MultipartFormDataContent multipartForm = new MultipartFormDataContent();

                multipartForm.Add(new StreamContent(avatarStream), "avatar", "avatar");

                var response = await httpClient.PostAsync(uri, multipartForm);
                
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                return responseJson;
            }
            catch(Exception ex)
            {
                //TODO: Log...
                //throw ex;
                return string.Empty;
            }
        }

        public async Task<bool> IsEmailFreeAsync(string email)
        {
            SetBasicToken();

            string responseContent = string.Empty;
            Uri uri = new Uri(urlRepository.GetIsEmailFree(email));
            responseContent= await httpClient.GetStringAsync(uri);
            return responseContent.Equals("true") ? true : false;
        }

        public async Task<User> UpdateUserAsync(User userToUpdate)
        {
            SetBasicToken();

            try
            {
                var uri = new Uri(urlRepository.PostUpdateUser());
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                var response = await httpClient.PostAsync(uri, CreateStringContent(userToUpdate));
                var responseContent = await response.Content.ReadAsStringAsync();

                response.EnsureSuccessStatusCode();

                var user = JsonConvert.DeserializeObject<User>(responseContent, jsonSerializerSettings);
                return user;
            }
            catch (Exception ex)
            {
                //TODO: Log....
                throw ex;
            }
            
        }

        public async Task<Tuple<string, string>> RefreshAccessToken(User user)
        {
            SetBasicToken();

            var keyValues = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", user.RefreshToken)
            };

            try
            {
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                Uri uri = new Uri(urlRepository.RefreshTokenUri);
                FormUrlEncodedContent httpContent = new FormUrlEncodedContent(keyValues);
                HttpResponseMessage response = await httpClient.PostAsync(uri, httpContent);

                response.EnsureSuccessStatusCode();

                string responseJson = await response.Content.ReadAsStringAsync();
                return ExtractTokensFromResponse(responseJson);
            }
            catch (Exception exception)
            {
                //TO DO: log...
                throw new Exception("Exception in UserService.RefreshAccessToken(...)", exception);
            }
        }

        public async Task<bool> IsAccessTokenStillValid(String accessToken)
        {
            SetBasicToken();

            var keyValues = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("token", accessToken)
            };

            try
            {
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                Uri uri = new Uri(urlRepository.CheckAccessTokenUri);
                FormUrlEncodedContent httpContent = new FormUrlEncodedContent(keyValues);
                HttpResponseMessage response = await httpClient.PostAsync(uri, httpContent);

                response.EnsureSuccessStatusCode();
                //Server will return StatusCode: 200 if accessToken is valid.

                return true;
            } 
            catch(Exception e)
            {
                //TODO: Log...
                //Server will return StatusCode: 404 if accessToken expired.
                return false;
            }
        }

        private Tuple<String, String> ExtractTokensFromResponse(String responseJson)
        {
            JObject jObjectResponse = JObject.Parse(responseJson);
            string accessToken = (string)jObjectResponse["access_token"];
            string refresToken = (string)jObjectResponse["refresh_token"];
            return new Tuple<string, string>(accessToken, refresToken);
        }

        private StringContent CreateStringContent<T>(T objectValue)
        {
            var jObject = JObject.FromObject(objectValue, camelCaseJsonSerializer);
            var stringContent = new StringContent(jObject.ToString(), Encoding.UTF8, "application/json");
            return stringContent;
        }

        private void ThrowExceptionWithErrorMessage(string responseJson)
        {
            var message = JObject.Parse(responseJson)["message"].ToString();
            throw new Exception(message);
        }
    }
}
