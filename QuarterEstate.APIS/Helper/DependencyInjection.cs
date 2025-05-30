

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quarter.Core;
using Quarter.Core.Entities.Identity;
using Quarter.Core.Mapping.Estates;
using Quarter.Core.ServiceContract;
using Quarter.Core.Services.Contract;
using Quarter.Repostory;
using Quarter.Repostory.Data.Context;
using Quarter.Repostory.Identity.Contexts;
using Quarter.Service.Service.Estates;
using Quarter.Service.Service.User;
using Quarter.Service.Tokens;
using QuarterEstate.APIS.Errors;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Store.APIS.Helper
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddSwaggerService();
            services.AddDbContextService(configuration);
            services.AddAutoMapperService(configuration);
            services.AddUserDefinedService();
            services.AddInvalidModelResponseService();
            services.AddAuthenticationService(configuration);
            services.AddCorsService(configuration);
            return services;
        }

        private static IServiceCollection AddBuiltInService(this IServiceCollection services)
        {
            services.AddControllers();
            return services;
        }

        private static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }

        private static IServiceCollection AddDbContextService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<QuarterDbContexts>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection12"));
            });
            services.AddDbContext<StoreIdentityDbContext>(option => {
                option.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            return services;
        }

        private static IServiceCollection AddUserDefinedService(this IServiceCollection services)
        {
            services.AddScoped<IProductService, EstateService>();
            services.AddScoped<IUnitofWork, UnitOfWork>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }

        private static IServiceCollection AddAutoMapperService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(M => M.AddProfile(new EstateProfile(configuration)));

            return services;
        }

        private static IServiceCollection AddInvalidModelResponseService(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState?.Where(p => p.Value?.Errors.Count > 0) // Added null checks to fix CS8602
                                                        .SelectMany(p => p.Value.Errors)
                                                        .Select(e => e.ErrorMessage)
                                                        .ToArray() ?? Array.Empty<string>(); // Added fallback to empty array

                    var response = new ApiValidationErorrResponse()
                    {
                        Erorrs = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });
            return services;
        }
       
        
        private static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IAuthenticationService), typeof(AuthenticationService));
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;



            }
                ).AddEntityFrameworkStores<StoreIdentityDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication(options =>
            {

                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }
                )
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidAudience = configuration["Jwt:Audience"],
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromDays(double.Parse(configuration["Jwt:DurationInDays"]))
                    };




                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                    policy.RequireRole("Admin"));

                options.AddPolicy("UserPolicy", policy =>
                    policy.RequireRole("User"));
            });
            return services;
        }
        private static IServiceCollection AddCorsService(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration["Cors:AllowedOrigins"]?
                         .Split(",", StringSplitOptions.RemoveEmptyEntries)
                         .Select(origin => origin.Trim())
                         .ToArray()
                     ?? new[] { "http://localhost:4200", "http://localhost:52156", "http://localhost:8080", "http://localhost:5000" };

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins(allowedOrigins)
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });

            return services;
        }


    }
}
