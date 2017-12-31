using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.DeviceInfo.Abstractions;
using SpotFinder.Core;
using SpotFinder.Models.DTO;
using SpotFinder.Repositories;
using System;
using System.Net.Http;
using System.Text;

namespace SpotFinder.DataServices
{
    public class ErrorService : IErrorService
    {
        private readonly HttpClient httpClient;
        private readonly IURLRepository urlRepository;
        private readonly IDeviceInfo deviceInfo;
        private readonly JsonSerializer camelCaseJsonSerializer;

        public ErrorService(HttpClient httpClient, IURLRepository urlRepository, IDeviceInfo deviceInfo, JsonSerializer camelCaseJsonSerializer)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.urlRepository = urlRepository ?? throw new ArgumentNullException(nameof(urlRepository));
            this.deviceInfo = deviceInfo ?? throw new ArgumentNullException(nameof(deviceInfo));
            this.camelCaseJsonSerializer = camelCaseJsonSerializer ?? throw new ArgumentNullException(nameof(camelCaseJsonSerializer));
        }

        public void SendErrorInformation(ErrorInfo errorInfo)
        {
            try
            {
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
