using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotFinder.Core;
using SpotFinder.Models.Core;
using SpotFinder.Models.DTO;
using SpotFinder.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.DataServices
{
    public class PlaceService : IPlaceService
    {
        private readonly HttpClient httpClient;
        private readonly IURLRepository urlRepository;
        private readonly JsonSerializer camelCaseJsonSerializer;

        public PlaceService(HttpClient httpClient, IURLRepository urlRepository, JsonSerializer camelCaseJsonSerializer)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.urlRepository = urlRepository ?? throw new ArgumentNullException(nameof(urlRepository));
            this.camelCaseJsonSerializer = camelCaseJsonSerializer ?? throw new ArgumentNullException(nameof(camelCaseJsonSerializer));
        }

        public async Task<IList<Place>> GetAllAsync()
        {
            List<Place> placeList = new List<Place>();
            try
            {
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                var uri = new Uri(urlRepository.GetPlacesUri);

                var response = await httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    placeList = new List<Place>();

                    var jArray = JArray.Parse(responseContent);
                    foreach (var item in jArray)
                    {
                        var placeWebLight = item.ToObject<PlaceWebLight>();
                        placeList.Add(Utils.PlaceWebLightToPlace(placeWebLight));
                    }
                }
                else
                {
                    throw new WebException(string.Format("{0}{1}", "Cannot download place from the server.", response.StatusCode));
                }

            }
            catch (Exception ex)
            {
                //TODO: Log...
                throw ex;
            }

            return placeList;
        }

        public async Task<int> SendAsync(Place place)
        {
            int result = 0;
            try
            {
                var placeDTO = Utils.PlaceToPlaceWeb(place);
                PrepareDTOToAdd(placeDTO);

                var jObject = JObject.FromObject(placeDTO, camelCaseJsonSerializer);
                var content = new StringContent(jObject.ToString(), Encoding.UTF8, "application/json");

                httpClient.Timeout = TimeSpan.FromSeconds(90);

                var uri = new Uri(urlRepository.PostPlaceUri);
                var response = await httpClient.PostAsync(uri, content);

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jObjectResponse = JObject.Parse(responseContent);
                    result = (int)jObjectResponse["id"];
                }
                else
                {
                    result = 0;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
        }

        public async Task<long> UpdateAsync(Place place, long placeId)
        {
            try
            {
                var uri = new Uri(urlRepository.PutPlaceUrl(placeId));

                var placeDTO = Utils.PlaceToPlaceWeb(place);
                var jObject = JObject.FromObject(placeDTO, camelCaseJsonSerializer);
                var content = new StringContent(jObject.ToString(), Encoding.UTF8, "application/json");

                httpClient.Timeout = TimeSpan.FromSeconds(30);

                var response = await httpClient.PutAsync(uri, content);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return long.Parse(responseContent);
            }
            catch (Exception ex)
            {
                //TODO: Log...
                throw ex;
            }
        }

        public async Task<IList<Place>> GetByCriteriaAsync(Criteria criteria)
        {
            var criteriaJson = JObject.FromObject(criteria, camelCaseJsonSerializer);
            var content = new StringContent(criteriaJson.ToString(), Encoding.UTF8, "application/json");
            var placesList = new List<Place>();

            try
            {
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                var uri = new Uri(urlRepository.GetPlaceByCriteriaUri);
                var response = await httpClient.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();

                    var jArray = JArray.Parse(stringResponse);
                    foreach (var item in jArray)
                    {
                        var placeWebLight = item.ToObject<PlaceWebLight>();
                        placesList.Add(Utils.PlaceWebLightToPlace(placeWebLight));
                    }
                }
                else
                {
                    throw new WebException(string.Format("{0}{1}", "Serwer response with errors. ", response.StatusCode));
                }

            }
            catch (Exception ex)
            {
                //TODO: Log...
                throw ex;    
            }

            return placesList;
        }

        public async Task<Place> GetByIdAsync(int id)
        {
            Place place;
            try
            {
                httpClient.Timeout = TimeSpan.FromSeconds(30);

                var uri = new Uri(urlRepository.GetPlaceByIdUri(id));
                var response = await httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jObjectPlace = JObject.Parse(responseContent);
                    place = Utils.PlaceWebToPlace(jObjectPlace.ToObject<PlaceWeb>());
                }
                else
                {
                    //TODO: Log...
                    throw new WebException(string.Format("{0}{1}", "Cannot download place from the server.", response.StatusCode));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error during geting all.");
                throw ex;
            }

            return place;
        }

        public async Task<IList<Place>> GetByUserIdAsync(long userId)
        {
            var uri = new Uri(urlRepository.GetPlacesListByUserIdUrl(userId));
            var placesList = new List<Place>();

            try
            {
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                var response = await httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();

                    var jArray = JArray.Parse(stringResponse);
                    foreach (var item in jArray)
                    {
                        var placeWebLight = item.ToObject<PlaceWebLight>();
                        placesList.Add(Utils.PlaceWebLightToPlace(placeWebLight));
                    }
                }
            }
            catch (Exception e)
            {
                //TODO: Log...
                throw e;
            }

            return placesList;
        }

        private void PrepareDTOToAdd(PlaceWeb placeDTO)
        {
            placeDTO.Id = null;

            foreach (var item in placeDTO.Images)
            {
                item.Id = null;
            }
        }
    }
}
