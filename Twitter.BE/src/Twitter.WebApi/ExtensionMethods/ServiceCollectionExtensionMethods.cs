using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Twitter.Data.Context;
using Twitter.Data.Model;
using Twitter.Repositories.Interfaces;
using Twitter.Repositories.Repositories;
using Twitter.Services.Helpers;
using Twitter.Services.Interfaces;
using Twitter.Services.Mappings;
using Twitter.Services.Options;
using Twitter.Services.Services;

namespace Twitter.WebApi.ExtensionMethods
{
    public static class ServiceCollectionExtensionMethods
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(setup =>
            {
                setup.DescribeAllParametersInCamelCase();
                setup.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Not A Twitter Web Api",
                    Version = "v1"
                });

                var apiScheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                };
                setup.AddSecurityDefinition("Bearer", apiScheme);
                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference 
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            In = ParameterLocation.Header,
                        }, new List<string>() 
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                setup.IncludeXmlComments(xmlPath);
            });
        }

        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<TwitterDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("TwitterConnection")));
        }

        public static void AddAndConfigureLocalization(this IServiceCollection services)
        {
            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en"),
                        new CultureInfo("pl"),
                    };

                    options.DefaultRequestCulture = new RequestCulture("en", "en");

                    options.SupportedCultures = supportedCultures;

                    options.SupportedUICultures = supportedCultures;
                });
        }
        public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtTokenOptions>(configuration.GetSection("JwtTokenOptions"));
            services.Configure<EmailServiceOptions>(configuration.GetSection("EmailSettings"));
            services.Configure<RedirectOptions>(configuration.GetSection("Redirects"));
        }

        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddDefaultIdentity<User>()
                .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
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

        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("JwtTokenOptions:JwtTokenSecret").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        public static void AddAndConfigureAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IEmailSenderService, EmailSenderService>();
            services.AddTransient<ITokenProviderService, JwtTokenProviderService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserContext, UserContext>();
            services.AddTransient<DataSeeder>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IBaseRepository<Follow>, BaseRepository<Follow>>();
            services.AddTransient<IBaseRepository<User>, BaseRepository<User>>();
            services.AddTransient<IBaseRepository<Language>, BaseRepository<Language>>();
            services.AddTransient<IBaseRepository<Gender>, BaseRepository<Gender>>();
        }
    }
}
