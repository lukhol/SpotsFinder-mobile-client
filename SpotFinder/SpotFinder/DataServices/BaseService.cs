using Redux;
using SpotFinder.Redux;
using SpotFinder.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SpotFinder.DataServices
{
    public class BaseService
    {
        protected readonly HttpClient httpClient;
        protected readonly URLRepository urlRepository;
        protected readonly IStore<ApplicationState> appStore;

        public BaseService(HttpClient httpClient, URLRepository urlRepository, IStore<ApplicationState> appStore)
        {
            this.appStore = appStore ?? throw new ArgumentNullException(nameof(appStore));
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.urlRepository = urlRepository ?? throw new ArgumentNullException(nameof(urlRepository));
        }

        protected void SetBasicToken()
        {
            var byteArray = Encoding.ASCII.GetBytes(urlRepository.API_KEY);
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray)
            );
        }

        protected void SetBearerToken()
        {
            httpClient.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("Bearer", appStore.GetState().UserState.User.AccessToken);
        }
    }
}
