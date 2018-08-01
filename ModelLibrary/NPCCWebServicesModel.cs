using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ModelLibrary
{
    public class NPCCWebServicesModel
    {
        public enum RequestType
        {
            Projects = 1
        }

        public enum ErrorType
        {
            ErrorOccurred = 0,
            InvalidToken = 1,
            InvalidRequestType = 2

        }
        public bool IsBusy { get; set; }

        public async Task<LoginInfo> Login(string username, string password)
        {
            try
            {
                if (IsBusy)
                    return null;

                string url = $"http://webapps.npcc.ae/Masharee/NPCCMobileWebServices/LoginValidator/?username={username}&password={password}";
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(url);
                var response = await client.GetAsync(client.BaseAddress);
                response.EnsureSuccessStatusCode();
                var jsonResult = response.Content.ReadAsStringAsync().Result;
                var Login_Info = JsonConvert.DeserializeObject<LoginInfo>(jsonResult);

                return Login_Info;
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
