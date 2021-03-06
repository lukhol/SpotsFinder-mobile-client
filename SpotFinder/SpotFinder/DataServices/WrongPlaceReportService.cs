﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotFinder.Redux.StateModels;
using SpotFinder.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.DataServices
{
    public class WrongPlaceReportService : IWrongPlaceReportService
    {
        private readonly HttpClient httpClient;
        private readonly URLRepository urlRepository;
        private readonly JsonSerializer camelCaseJsonSerializer;

        public WrongPlaceReportService(HttpClient httpClient, URLRepository urlRepository, JsonSerializer camelCaseJsonSerializer)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(HttpClient));
            this.urlRepository = urlRepository ?? throw new ArgumentNullException(nameof(urlRepository));
            this.camelCaseJsonSerializer = camelCaseJsonSerializer ?? throw new ArgumentNullException(nameof(camelCaseJsonSerializer));
        }

        public async Task SendAsync(WrongPlaceReport wrongPlaceReport)
        {
            try
            {
                var uri = new Uri(urlRepository.PostWrongPlaceReportUri);
                var response = await httpClient.PostAsync(uri, CreateStringContent(wrongPlaceReport));

                if (response.StatusCode == HttpStatusCode.OK)
                    return;

                if(response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var message = JObject.Parse(responseJson)["message"].ToString();
                    throw new Exception(message);
                }
            }
            catch (Exception e)
            {
                //TODO: Log...
                throw new Exception(e.Message);
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
