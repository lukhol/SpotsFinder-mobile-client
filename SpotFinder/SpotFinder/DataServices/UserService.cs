using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotFinder.Redux.StateModels;
using SpotFinder.Repositories;
using System;
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
                    var user = JsonConvert.DeserializeObject<User>(responseContent);
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
