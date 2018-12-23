using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Resilience;
using User.Identity.Authorization;
using User.Identity.Dtos;
using User.Identity.Infrastructure;
using User.Identity.Service;

namespace User.Identity
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddExtensionGrantValidator<SmsAuthCodeValidate>() //添加自定义验证
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryIdentityResources(Config.GetIdentityResources());
            services.AddSingleton<IAuthCodeService, TestAuthCodeService>()
                .AddSingleton<IUserService, UserApiService>();
            services.AddSingleton<SmsAuthCodeValidate>();
            /*services.AddHttpClient()*/;
            //注册全局单例IHttpClient
            services.AddSingleton<ResilienceHttpClientFactory>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ResilienceHttpClient>>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                if (!int.TryParse(Configuration["Polly:RetryCount"], out int retryCount))
                {
                    retryCount = 4;
                }
                if (!int.TryParse(Configuration["Polly:CircuitCountAllowedBeforeException"],out int circuitAllowedBeforeException))
                {
                    circuitAllowedBeforeException = 5;
                }
                return new ResilienceHttpClientFactory(logger,httpContextAccessor, retryCount, circuitAllowedBeforeException);
            });
            services.AddSingleton<IHttpClient>(sp =>
                sp.GetRequiredService<ResilienceHttpClientFactory>().GetResilienceHttpClient());
            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            services.AddSingleton<IDnsQuery>(s =>
            {
                var serviceConfiguration = s.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;
                return new LookupClient(serviceConfiguration.Consul.DnsEndpoint.ToIPEndPoint());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseIdentityServer();
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}
