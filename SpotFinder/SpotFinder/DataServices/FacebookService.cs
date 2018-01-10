using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpotFinder.Models.DTO;
using SpotFinder.Repositories;

namespace SpotFinder.DataServices
{
    public class FacebookService : IFacebookService
    {
        private readonly IURLRepository urlRepository;
        private readonly JsonSerializer camelCaseJsonSerializer;

        public FacebookService(IURLRepository urlRepository, JsonSerializer camelCaseJsonSerializer)
        {
            this.urlRepository = urlRepository ?? throw new ArgumentNullException(nameof(urlRepository));
            this.camelCaseJsonSerializer = camelCaseJsonSerializer ?? throw new ArgumentNullException(nameof(camelCaseJsonSerializer));
        }

        public async Task<SimpleFacebookDTO> GetSimpleFacebookInfoAsync(string accessToken)
        {
            try
            {
                Uri uri = new Uri(urlRepository.GetFacebookInfoUri(accessToken));

                using (var httpClient = new HttpClient())
                {
                    var userJson = await httpClient.GetStringAsync(uri);
                    var simpleFacebookDTO = JsonConvert.DeserializeObject<SimpleFacebookDTO>(userJson);
                    return simpleFacebookDTO;
                }
            }
            catch(Exception exception)
            {
                //TODO: Log...
                throw exception;
            }
        }
    }
}
