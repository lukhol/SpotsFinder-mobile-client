using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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

        public List<Place> GetAllPlace()
        {
            return placeList;
        }

        public bool Send(Place place)
        {
            //Transform place to json - DONE

            //Send json to serwer
            //Get response and return proper value

            //Save to local database this json - DONE BUT WRONG WAY (ID)

            var jObject = JObject.FromObject(place);
            var jsonSpotString = jObject.ToString();

            LocalPlaceRepository.InsertPlace(jsonSpotString);

            return true;
        }

        public async Task<List<Place>> GetPlacesByCriteria(Criteria criteria)
        {
            var criteriaJson = JObject.FromObject(criteria);
            var content = new StringContent(criteriaJson.ToString(), Encoding.UTF8, "application/json");

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var uri = new Uri("http://demo5878347.mockable.io/spots/criteria");
                    var response =  await httpClient.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        placeList = new List<Place>();
                        var stringResponse = await response.Content.ReadAsStringAsync();

                        var jArray = JArray.Parse(stringResponse);
                        foreach(var item in jArray)
                        {
                            placeList.Add(item.ToObject<Place>());
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
