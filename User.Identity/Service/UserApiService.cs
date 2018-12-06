using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using User.Identity.Dtos;

namespace User.Identity.Service
{
    public class UserApiService : IUserService
    {
        private readonly string _baseUrl = "";
        private readonly HttpClient _httpClient;


        public UserApiService(HttpClient httpClient, IDnsQuery dnsQuery, IOptions<ServiceDiscoveryOptions> options)
        {
            _httpClient = httpClient;
            ServiceHostEntry[] result = dnsQuery.ResolveService("service.consul", options.Value.UserServiceName);
            var addressList = result.First().AddressList;
            var address = addressList.Any() ? addressList.First().ToString() : result.First().HostName;
            var port = result.First().Port;
            _baseUrl = $"http://{address}:{port}/";
        }

        public async Task<int> CheckOrCreate(string phone)
        {
            var userId = 0;
            string api = _baseUrl + "api/users/create-or-update";

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