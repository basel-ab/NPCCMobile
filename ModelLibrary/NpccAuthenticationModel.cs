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
using Xamarin.Essentials;
using static ModelLibrary.clsEnum;

namespace ModelLibrary
{
    public class  Authentication
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

        public async Task<bool> IsAuthenticatedCheckAsync()
        {
            var oauthToken = await SecureStorage.GetAsync("oauth_token");
            return false;
        }
    }
}