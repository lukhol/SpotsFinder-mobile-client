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
using SpotFinder.Models.WebModels;

namespace SpotFinder.Core
{
    public class PlaceRepository : IPlaceRepository
    {
        private ILocalPlaceRepository LocalPlaceRepository { get; }

        private List<Place> placeList = new List<Place>();

        public PlaceRepository(ILocalPlaceRepository localPlaceRepository)
        {
            LocalPlaceRepository = localPlaceRepository ?? throw new ArgumentNullException("localPlaceRepository is null in PlaceRepository");
        }

        public async Task<List<Place>> GetAllPlace()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var uri = new Uri(GlobalSettings.GetAllUrl);
                    var response = await httpClient.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        placeList = new List<Place>();

                        var jArray = JArray.Parse(responseContent);
                        foreach (var item in jArray)
                        {
                            var placeWeb = item.ToObject<PlaceWeb>();
                            placeList.Add(Utils.PlaceWebToPlace(placeWeb));
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
                var jObject = JObject.FromObject(Utils.PlaceToPlaceWeb(place), GlobalSettings.GetCamelCaseSerializer());
                var content = new StringContent(jObject.ToString(), Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
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

                await LocalPlaceRepository.InsertPlace(jObject.ToString());
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
        }

        public async Task<List<Place>> GetPlacesByCriteria(Criteria criteria)
        {
            var criteriaJson = JObject.FromObject(criteria, GlobalSettings.GetCamelCaseSerializer());
            var content = new StringContent(criteriaJson.ToString(), Encoding.UTF8, "application/json");

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var uri = new Uri(GlobalSettings.GetPlaceByCriteriaUrl);
                    var response =  await httpClient.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        placeList = new List<Place>();
                        var stringResponse = await response.Content.ReadAsStringAsync();

                        var jArray = JArray.Parse(stringResponse);
                        foreach(var item in jArray)
                        {
                            var placeWeb = item.ToObject<PlaceWeb>();
                            placeList.Add(Utils.PlaceWebToPlace(placeWeb));
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
                Debug.WriteLine("Error during getting places with categories.", e.Message);
                return null;
            }

            return placeList;
        }
    }
}
