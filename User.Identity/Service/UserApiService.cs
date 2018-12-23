using System;
using System.Collections.Generic;
using DnsClient;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Resilience;
using User.Identity.Dtos;

namespace User.Identity.Service
{
    public class UserApiService : IUserService
    {
        private readonly string _baseUrl = "";
        private readonly IHttpClient _client;
        private readonly ILogger<UserApiService> _logger;

        public UserApiService(IHttpClient client, IDnsQuery dnsQuery, IOptions<ServiceDiscoveryOptions> options, ILogger<UserApiService> logger)
        {
            _client = client;
            _logger = logger;
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
            Dictionary<string,string> dic = new Dictionary<string, string>
            {
                {"Phone",phone }
            };
            try
            {
                var response = await _client.PostAsync(api, dic);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultCode = await response.Content.ReadAsStringAsync();
                    int.TryParse(resultCode, out userId);
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning($"重试失败:{e.Message} {e.StackTrace}");
                throw;
            }
            return userId;
        }
    }
}