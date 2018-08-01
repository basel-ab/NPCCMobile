using System;
using System.Net.Http;
using System.Text;
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
            if (IsBusy)
                return null;

            HttpClient client = new HttpClient();
            string url = $"http://webapps.npcc.ae/Masharee/NPCCMobileWebServices/LoginValidator";
            client.BaseAddress = new Uri(url);

            clsCredentials objCredentials = new clsCredentials();
            objCredentials.username = username;
            objCredentials.password = password;

            string json = JsonConvert.SerializeObject(objCredentials);

            try
            {
                var content = new StringContent(json, Encoding.UTF32, "application/json");

                var response = await client.PostAsync(client.BaseAddress, content);
                response.EnsureSuccessStatusCode();
                var jsonResult = response.Content.ReadAsStringAsync().Result;
                var Login_Info = JsonConvert.DeserializeObject<LoginInfo>(jsonResult);

                return Login_Info;
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return null;
            }
            finally
            {
                IsBusy = false;
            }

        }

        public static async Task<dynamic> sendMessage()
        {

            HttpClient client = new HttpClient();
            var uri = new Uri("http://127.0.0.1:80/ProgettoProfiloUtenti/uno.php/");


            ProjectInfo lgg = new LoginInfo();

            lgg.Token="dadasdasd";
            lgg. = new DateTime(2008, 12, 28);






            return "";
        }
    }
}
