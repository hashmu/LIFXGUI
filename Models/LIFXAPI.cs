using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LIFXGUI.Models
{
    class LIFXAPI
    {
        #region Constructors
        public LIFXAPI(string token)
        {
            _token = token;
        }
        #endregion

        #region Members
        private string _token = "";
        #endregion


        #region Methods
        public async Task<string> CurrentSettings()
        {
            string settings = await GetRequest();
            return settings;
        }

        public async Task<string> GetRequest()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                var response = await client.GetAsync("https://api.lifx.com/v1/lights/all");
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
                return responseString;
            }
        }

        public async Task<string> Toggle()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                var response = await client.PostAsync("https://api.lifx.com/v1/lights/all/toggle", null);
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> Toggle(bool toggle)
        {
            var values = new Dictionary<string, string>
            {
                {"power", toggle ? "on" : "off" },
                {"fast", "true" }
            };
            return await PutRequest(values);
        }

        public async Task<string> Set(int colour, string brightness)
        {
            var values = new Dictionary<string, string>
            {
                { "power", "on" },
                { "color", "kelvin:" + colour.ToString() },
                { "brightness", brightness },
                { "fast", "true" }
            };
            return await PutRequest(values);
        }

        private async Task<string> PutRequest(Dictionary<string, string> values)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                var content = new FormUrlEncodedContent(values);
                var response = await client.PutAsync("https://api.lifx.com/v1/lights/all/state", content);
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
                return response.StatusCode.ToString();
            }
        }

        private string HandleStatusCode(int code)
        {
            string response = "";
            switch (code)
            {
                case 200:
                    response = "Success";
                    break;
                default:
                    response = "Failed: " + code;
                    break;
            }
            return response;
        }
        #endregion
    }
}
