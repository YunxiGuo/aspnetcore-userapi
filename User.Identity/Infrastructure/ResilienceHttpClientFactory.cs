using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Resilience;
using System;
using System.Net.Http;

namespace User.Identity.Infrastructure
{
    /// <summary>
    /// 创建ResilienceHttpClient实例
    /// </summary>
    public class ResilienceHttpClientFactory
    {
        private readonly ILogger<ResilienceHttpClient> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 重试次数
        /// </summary>
        private readonly int _retryCount;

        /// <summary>
        /// 熔断之前允许异常发生次数
        /// </summary>
        private readonly int _circuitCountAllowedBeforeException;

        public ResilienceHttpClientFactory(
            ILogger<ResilienceHttpClient> logger,
            IHttpContextAccessor httpContextAccessor,
            int retryCount,
            int circuitCountAllowedBeforeException)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _retryCount = retryCount;
            _circuitCountAllowedBeforeException = circuitCountAllowedBeforeException;
        }

        public ResilienceHttpClient GetResilienceHttpClient()
        {
            return new ResilienceHttpClient(CreatePolicies, _logger, _httpContextAccessor);
        }

        private Policy[] CreatePolicies(string origin)
        {
            return new Policy[]
            {
                Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(
                    _retryCount,
                    attempt => TimeSpan.FromSeconds(Math.Pow(2,attempt)),
                    (exception,timeSpan,retryCount,context) =>
                    {
                        var msg = $"第{retryCount}次重试" +
                                  $"of {context.PolicyKey}" +
                                  $"at {context.OperationKey}" +
                                  $"due to {exception}";
                        _logger.LogWarning(msg);
                        _logger.LogDebug(msg);
                    }
                    ),
                Policy.Handle<HttpRequestException>()
                .CircuitBreakerAsync(
                    _circuitCountAllowedBeforeException,
                    TimeSpan.FromMinutes(1),
                    (exception, duration) =>
                    {
                        _logger.LogTrace("熔断器断开");
                    }, () =>
                    {
                        _logger.LogTrace("熔断器关闭");
                    })
            };
        }
    }
}