using Newtonsoft.Json.Linq;
using SpotFinder.Core;
using SpotFinder.Models.Core;
using SpotFinder.Models.DTO;
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

        public PlaceService(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<Place>> GetAllPlacesAsync()
        {
            List<Place> placeList = new List<Place>();
            try
            {
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                var uri = new Uri(GlobalSettings.GetAllUrl);
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

                var jObject = JObject.FromObject(placeDTO, GlobalSettings.GetCamelCaseSerializer());
                var content = new StringContent(jObject.ToString(), Encoding.UTF8, "application/json");

                var strinJObject = jObject.ToString();

                httpClient.Timeout = TimeSpan.FromSeconds(90);
                var uri = new Uri(GlobalSettings.PostSpotUrl);
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

        public async Task<List<Place>> GetPlacesByCriteriaAsync(Criteria criteria)
        {
            var criteriaJson = JObject.FromObject(criteria, GlobalSettings.GetCamelCaseSerializer());
            var content = new StringContent(criteriaJson.ToString(), Encoding.UTF8, "application/json");
            var placesList = new List<Place>();

            try
            {
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                var uri = new Uri(GlobalSettings.GetPlaceByCriteriaUrl);
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

        public async Task<Place> GetPlaceByIdAsync(int id)
        {
            Place place;
            try
            {
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                var uri = new Uri(GlobalSettings.GetByIdUrl + id.ToString());
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
