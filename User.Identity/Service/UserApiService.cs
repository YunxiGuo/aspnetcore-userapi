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
        private readonly HttpClient _httpClient;

        public UserApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> CheckOrCreate(string phone)
        {
            var userId = 0;
            string api = BaseUrl + "api/users/create-or-update";

            var param = new
            {
                Phone = phone
            };
            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(param)); //GetEncoding("GBK")
            Stream stream = new MemoryStream(bytes);
            StreamContent content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json") { CharSet = "UTF-8" };

            var response = await _httpClient.PostAsync(api, content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string resultCode = await response.Content.ReadAsStringAsync();
                int.TryParse(resultCode, out userId);
            }
            return userId;
        }
    }
}