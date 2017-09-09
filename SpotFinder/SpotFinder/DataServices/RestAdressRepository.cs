using System;
using Plugin.Geolocator.Abstractions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace SpotFinder.DataServices
{
    public class RestAdressRepository : IRestAdressRepository
    {
        public async Task<Position> GetPositionOfTheCity(string cityName, bool sensor)
        {
            cityName = cityName.Trim();

            if (string.IsNullOrEmpty(cityName))
                return null;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var urlBuilder = new StringBuilder();
                    urlBuilder.Append("http://maps.googleapis.com/maps/api/geocode/json?address=");
                    urlBuilder.Append(cityName);
                    urlBuilder.Append("&sensor=");
                    urlBuilder.Append(sensor.ToString());

                    var uri = new Uri(urlBuilder.ToString());

                    var response = await httpClient.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            var stringResponse = await response.Content.ReadAsStringAsync();

                            var jObject = JObject.Parse(stringResponse);
                            var locationToken = jObject["results"][0]["geometry"]["location"];

                            var position = new Position();

                            double lat = double.Parse(locationToken["lat"].ToString());
                            double lng = double.Parse(locationToken["lng"].ToString());

                            position.Latitude = lat;
                            position.Longitude = lng;

                            return position;
                        }
                        catch
                        {
                            throw new Exception("Error during parsing");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }
    }
}
