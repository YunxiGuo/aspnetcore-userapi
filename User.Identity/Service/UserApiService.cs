using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace User.Identity.Service
{
    public class UserApiService : IUserService
    {
        private const string BaseUrl = "http://127.0.0.1:5002/";

        public async Task<int> CheckOrCreate(string phone)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string api = BaseUrl + "api/users/create-or-update";

                    var param = new
                    {
                        Phone = "13111111111"
                    };
                    byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(param)); //GetEncoding("GBK")
                    Stream stream = new MemoryStream(bytes);
                    StreamContent content = new StreamContent(stream);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json") {CharSet = "UTF-8"};

                    Task<HttpResponseMessage> response = client.PostAsync(api, content);

                    if (response.Result.StatusCode == HttpStatusCode.OK)
                    {
                        string resultCode = response.Result.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        
                    }
                }
            }
            catch (Exception exception)
            {
                //BALABALBALLBA
            }
            return 1;
        }
    }
}