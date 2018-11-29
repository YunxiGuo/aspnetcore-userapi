using System;
using System.Linq;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using User.API.Data;
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
            services.AddMvc(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            //服务注册
            applicationLifetime.ApplicationStarted.Register(ConsulRegister);
            //服务注销
            applicationLifetime.ApplicationStopping.Register(() =>
            {
                //consulClient.Agent.ServiceDeregister(registration.ID).Wait();//服务停止时取消注册
            });
            app.UseMvc();
            AppUserInitSeed(app);
        }

        private void ConsulRegister()
        {
            string ip = Configuration["service:ip"];
            string port = Configuration["service:port"];
            string serviceName = Configuration["service:name"];
            string serviceId = serviceName + Guid.Parse(_userApiId);

            var consulClient = new ConsulClient(m =>
                m.Address = new Uri(
                    $"http://{Configuration["ServcieDiscovery:Address"]}:{Configuration["ServcieDiscovery:Port"]}"));
            var httpCheck = new AgentServiceCheck
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                HTTP = $"http://{ip}:{port}/api/health",//健康检查地址
                Timeout = TimeSpan.FromSeconds(5)
            };
            // Register service with consul

            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = serviceId,
                Name = serviceName,
                Address = ip,
                Port = Convert.ToInt32(port),
                Tags = new[] { $"urlprefix-/{serviceName}" }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };

            consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）      
        }

        private void ConsulDeRegister()
        {

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
