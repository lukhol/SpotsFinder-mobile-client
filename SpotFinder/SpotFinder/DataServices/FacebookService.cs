using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpotFinder.Models.DTO;
using SpotFinder.Repositories;

namespace SpotFinder.DataServices
{
    public class FacebookService : IExternalUserService<SimpleFacebookUserDTO>
    {
        private readonly IURLRepository urlRepository;
        private readonly JsonSerializer camelCaseJsonSerializer;

        public FacebookService(IURLRepository urlRepository, JsonSerializer camelCaseJsonSerializer)
        {
            this.urlRepository = urlRepository ?? throw new ArgumentNullException(nameof(urlRepository));
            this.camelCaseJsonSerializer = camelCaseJsonSerializer ?? throw new ArgumentNullException(nameof(camelCaseJsonSerializer));
        }

        [Obsolete("Facebook does not require to get access_token using code value!")]
        public Task<string> GetAccessTokenAsync(string code)
        {
            throw new NotImplementedException();
        }

        public async Task<SimpleFacebookUserDTO> GetExternalUserInfoAsync(string accessToken)
        {
            try
            {
                Uri uri = new Uri(urlRepository.GetFacebookUserInfoUri(accessToken));

                using (var httpClient = new HttpClient())
                {
                    var userJson = await httpClient.GetStringAsync(uri);
                    dynamic dynamicFacebookUser = JsonConvert.DeserializeObject(userJson, typeof(object));

                    return new SimpleFacebookUserDTO
                    {
                        Id = dynamicFacebookUser.@id,
                        Email = dynamicFacebookUser.@email,
                        Firstname = dynamicFacebookUser.@first_name,
                        Lastname = dynamicFacebookUser.@last_name,
                        AvatarUrl = dynamicFacebookUser.@picture.@data.@url
                    };
                }
            }
            catch(Exception exception)
            {
                //TODO: Log...
                throw new Exception("Exception in FacebookService.GetExternalUserInfoAsync(accessToken).", exception);
            }
        }
    }
}
