using System;
using System.Linq;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using User.API.Data;
using User.API.Dtos;
using User.API.Filters;
using User.API.Models;

namespace User.API
{
    public class Startup
    {
        private static string _userApiId = "FB89478E-632A-4676-B925-27E64F9987B7";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserContext>(options =>
            {
                //options.UseMySQL(Configuration.GetConnectionString("UserApi"));
                options.UseNpgsql(Configuration.GetConnectionString("UserApi"));
            });
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication("Bearer", option =>
                {
                    option.Authority = "http://localhost:5003";
                    option.RequireHttpsMetadata = false;
                    option.ApiName = "user_api";
                });

            services.AddOptions();
            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            //注册consul单例
            services.AddSingleton<IConsulClient>(p => new ConsulClient(config =>
                {
                    var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;
                    if (string.IsNullOrWhiteSpace(serviceConfiguration.Consul.HttpEndpoint))
                    {
                        serviceConfiguration.Consul.HttpEndpoint = "http://127.0.0.1:8500";
                    }
                    config.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                })
            );
            services.AddMvc(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            IApplicationLifetime applicationLifetime,
            IOptions<ServiceDiscoveryOptions> serviceOptions,
            IConsulClient consul)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            //程序启动时注册服务
            applicationLifetime.ApplicationStarted.Register(() =>
            {
                ConsulRegister(app, consul, serviceOptions);
            });
            //程序停止时注销服务
            applicationLifetime.ApplicationStopping.Register(() =>
            {
                ConsulDeRegister(app,consul,serviceOptions);
            });
            app.UseMvc();
            AppUserInitSeed(app);
        }

        private void ConsulRegister(
            IApplicationBuilder app,
            IConsulClient consul, 
            IOptions<ServiceDiscoveryOptions> serviceOptions)
        {

            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(p => new Uri(p));

            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";

                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(10),
                    HTTP = $"http://{address.Host}:{address.Port}/api/health",//健康检查地址
                    Timeout = TimeSpan.FromSeconds(5)
                };

                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    Address = address.Host,
                    ID = serviceId,
                    Name = serviceOptions.Value.ServiceName,
                    Port = address.Port,
                    Tags = new[] { $"urlprefix-/{serviceOptions.Value.ServiceName}" }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
                };

                consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
            }      
        }

        private void ConsulDeRegister(
            IApplicationBuilder app,
            IConsulClient consul, 
            IOptions<ServiceDiscoveryOptions> serviceOptions)
        {
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(p => new Uri(p));
            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";

                consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
            }
        }

        private void AppUserInitSeed(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userContext = scope.ServiceProvider.GetService<UserContext>();
                if (!userContext.Database.EnsureCreated())
                {
                    if (!userContext.AppUsers.Any())
                    {
                        userContext.AppUsers.Add(new AppUser
                        {
                            Name = "guoyunxi"
                        });
                        userContext.SaveChanges();
                    }
                }
            }
        }
    }
}
