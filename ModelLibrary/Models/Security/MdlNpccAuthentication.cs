using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;

namespace ModelLibrary
{
    public class  MdlNpccAuthentication
    {
        public bool IsBusy { get; set; }

        public static String GetMacAddress()
        {
            string mac = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {

                if (nic.OperationalStatus == OperationalStatus.Up && (!nic.Description.Contains("Virtual") && !nic.Description.Contains("Pseudo")))
                {
                    if (nic.GetPhysicalAddress().ToString() != "")
                    {
                        mac = nic.GetPhysicalAddress().ToString();
                    }
                }
            }

            return mac;
        }

        public async Task<clsLoginInfo> Login(string username, string password)
        {
            if (IsBusy)
                return null;

            HttpClient client = new HttpClient();
            string url = "https://webapps.npcc.ae/ApplicationWebServices/api/Authentication/LoginValidator";
            client.BaseAddress = new Uri(url);

            clsCredentials objCredentials = new clsCredentials();
            objCredentials.username = username;
            objCredentials.password = password;

            string json = JsonConvert.SerializeObject(objCredentials);

            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(client.BaseAddress, content);
                response.EnsureSuccessStatusCode();
                var jsonResult = response.Content.ReadAsStringAsync().Result;
                var Login_Info = JsonConvert.DeserializeObject<clsLoginInfo>(jsonResult);

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

        public async Task<bool> IsAuthenticatedAsync()
        {
            var oauthToken = await SecureStorage.GetAsync("oauth_token");

            HttpClient client = new HttpClient();
            string url = "https://webapps.npcc.ae/ApplicationWebServices/api/Authentication/IsAuthenticated";
            client.BaseAddress = new Uri(url);

            //JObject oJsonObject = new JObject();
            //oJsonObject.Add("Token",  oauthToken); 

            clsToken oToken = new clsToken();
            oToken.Token = oauthToken;

            string json = JsonConvert.SerializeObject(oToken);

            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(client.BaseAddress, content);
                response.EnsureSuccessStatusCode();
                var jsonResult = response.Content.ReadAsStringAsync().Result;
                bool isAuthenticated = JsonConvert.DeserializeObject<bool>(jsonResult);

                if(!isAuthenticated) SecureStorage.Remove("oauth_token");
                return isAuthenticated;
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return false;
            }
        }
    }
}