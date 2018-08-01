using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ModelLibrary
{
    public class Model
    {
        public bool IsBusy { get; set; }

        public async Task<WeatherResult> GetWeatherReportAsync(double latitude, double longitude)
        {
            try
            {
                if (IsBusy)
                    return null;
                
                string url = $"http://api.geonames.org/findNearByWeatherJSON?formatted=true&lat={latitude}&lng={longitude}&username=sekharonline4u&style=full";
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(url);
                var response = await client.GetAsync(client.BaseAddress);
                response.EnsureSuccessStatusCode();
                var jsonResult = response.Content.ReadAsStringAsync().Result;
                var weather = JsonConvert.DeserializeObject<WeatherResult>(jsonResult);

                return weather;
            }
            catch (Exception ex)
            {
                return null;
            } 
            finally
            {
                IsBusy = false;
            }

        }
    }
}
