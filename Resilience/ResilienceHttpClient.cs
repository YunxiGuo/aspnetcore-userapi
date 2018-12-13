using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Wrap;

namespace Resilience
{
    public class ResilienceHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// 根据url origin去创建policy
        /// </summary>
        private readonly Func<string, IEnumerable<Policy>> _policyCreator;

        /// <summary>
        /// 把policy打包成组合policy wraper,进行本地缓存
        /// </summary>
        private readonly ConcurrentDictionary<string, PolicyWrap> _policyWraps;

        private ILogger<ResilienceHttpClient> _logger;

        private IHttpContextAccessor _httpContextAccessor;

        public ResilienceHttpClient(
            Func<string, IEnumerable<Policy>> policyCreator, 
            ILogger<ResilienceHttpClient> logger, 
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = new HttpClient();
            _policyWraps = new ConcurrentDictionary<string, PolicyWrap>();
            _policyCreator = policyCreator;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string url, T item, string authorizationToken, string requestId = null,
            string authorizationMethod = "Bearer")
        {
            throw new System.NotImplementedException();
        }

        private Task<HttpResponseMessage> DoPostAsync<T>(HttpMethod httpMethod, string url, T item,
            string authorizationToken, string requestId = null, string authorizationMethod = "Bearer")
        {
            if (httpMethod != HttpMethod.Post && httpMethod != HttpMethod.Put)
            {
                throw new ArgumentException("value must be post or put", nameof(httpMethod));
            }

            var origin = GetOriginFromUri(url);
            return null;
            //return HttpMessageInvoker()
        }

        private static string NormalizeOrigin(string origin)
        {
            return origin?.Trim()?.ToLower();
        }

        private static string GetOriginFromUri(string uri)
        {
            var url = new Uri(uri);
            var origin = $"{url.Scheme}://{url.DnsSafeHost}:{url.Port}";
            return origin;
        }

        private void SetAuthorizationHeader(HttpRequestMessage requestMessage)
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrWhiteSpace(authorizationHeader))
            {
                requestMessage.Headers.Add("Authorization", new List<string>() {authorizationHeader});
            }
        }
    }
}