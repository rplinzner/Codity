using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Twitter.Services.Helpers;
using Twitter.Services.Hubs;
using Twitter.WebApi.ExtensionMethods;

namespace Twitter.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwagger();
            services.AddSignalR();
            services.AddDbContext(Configuration);
            services.AddAuthentication(Configuration);
            services.AddAndConfigureLocalization();
            services.AddIdentity();
            services.AddAndConfigureAutoMapper();
            services.AddServices();
            services.ConfigureOptions(Configuration);
            services.AddRepositories();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataSeeder dataSeeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                dataSeeder.EnsureSeedData();
            }

            app.UseAuthentication();
            app.ConfigureSwagger();
            app.UseHttpsRedirection();
            app.UseRequestLocalization();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/notificationHub");
            });
        }
    }
}
