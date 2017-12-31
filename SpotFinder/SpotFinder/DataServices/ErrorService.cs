using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.DeviceInfo.Abstractions;
using SpotFinder.Models.DTO;
using SpotFinder.Repositories;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SpotFinder.DataServices
{
    public class ErrorService : IErrorService
    {
        private readonly HttpClient httpClient;
        private readonly IURLRepository urlRepository;
        private readonly JsonSerializer camelCaseJsonSerializer;

        public ErrorService(HttpClient httpClient, IURLRepository urlRepository, JsonSerializer camelCaseJsonSerializer)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.urlRepository = urlRepository ?? throw new ArgumentNullException(nameof(urlRepository));
            this.camelCaseJsonSerializer = camelCaseJsonSerializer ?? throw new ArgumentNullException(nameof(camelCaseJsonSerializer));
        }

        public void SendErrorInformation(ErrorInfo errorInfo)
        {
            try
            {
                var byteArray = Encoding.ASCII.GetBytes(urlRepository.API_KEY);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                var errorUri = new Uri(urlRepository.PostErrorUri);
                var json = JObject.FromObject(errorInfo, camelCaseJsonSerializer).ToString();
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpClient.PostAsync(errorUri, content);
            }
            catch(Exception e)
            {
                //TODO: Log...
            }
        }
    }
}
