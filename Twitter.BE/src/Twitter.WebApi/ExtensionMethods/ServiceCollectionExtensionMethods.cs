using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Twitter.Data.Context;
using Twitter.Data.Model;

namespace Twitter.WebApi.ExtensionMethods
{
    public static class ServiceCollectionExtensionMethods
    {
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<TwitterDbContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("TwitterConnection")));
        }

        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddDefaultIdentity<User>()
              .AddEntityFrameworkStores<TwitterDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = true;

                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 0;
            });
        }

        public static void AddServices(this IServiceCollection services)
        {
        }

        public static void AddRepositories(this IServiceCollection services)
        {
        }
    }
}
