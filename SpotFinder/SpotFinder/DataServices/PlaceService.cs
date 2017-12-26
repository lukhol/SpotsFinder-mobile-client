using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using SpotFinder.Models.DTO;
using SpotFinder.Models.Core;
using SpotFinder.Core;

namespace SpotFinder.DataServices
{
    public class PlaceService : IPlaceService
    {
        private List<Place> placeList = new List<Place>();

        public async Task<List<Place>> GetAllPlaceAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
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
                        return null;
                    }
                }
            }
            catch
            {
                Debug.WriteLine("Error during geting all.");
                return null;
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

                using (var httpClient = new HttpClient())
                {
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
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
        }

        public async Task<List<Place>> GetPlacesByCriteriaAsync(Criteria criteria)
        {
            var criteriaJson = JObject.FromObject(criteria, GlobalSettings.GetCamelCaseSerializer());
            var content = new StringContent(criteriaJson.ToString(), Encoding.UTF8, "application/json");

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    var uri = new Uri(GlobalSettings.GetPlaceByCriteriaUrl);
                    var response =  await httpClient.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        placeList = new List<Place>();
                        var stringResponse = await response.Content.ReadAsStringAsync();

                        var jArray = JArray.Parse(stringResponse);
                        foreach(var item in jArray)
                        {
                            var placeWebLight = item.ToObject<PlaceWebLight>();
                            placeList.Add(Utils.PlaceWebLightToPlace(placeWebLight));
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch(Exception e)
            {
                //Tu trafia jak serwer nie odpowiada oraz jak internet jest wyłączony
                Debug.WriteLine("Error during getting places with categories.", e.Message);

                throw new WebException("No internet or server error");
            }

            return placeList;
        }

        public async Task<Place> GetPlaceByIdAsync(int id)
        {
            Place place;
            try
            {
                using (var httpClient = new HttpClient())
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
                        return null;
                    }
                }
            }
            catch
            {
                Debug.WriteLine("Error during geting all.");
                return null;
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
