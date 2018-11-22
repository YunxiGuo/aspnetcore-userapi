using System.Linq;
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();
            AppUserInitSeed(app);
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
