using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace User.Identity.Service
{
    public class UserApiService : IUserService
    {
        private const string BaseUrl = "http://127.0.0.1:5002/";

        public async Task<int> CheckOrCreate(string phone)
        {
            var userId = 0;
            //通过http请求调用User.API的CheckOrCreate接口
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get,
                new Uri(BaseUrl + "api/users/create-or-update")));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (int.TryParse(content,out userId))
                {
                    
                }
            }
            return userId;
        }
    }
}