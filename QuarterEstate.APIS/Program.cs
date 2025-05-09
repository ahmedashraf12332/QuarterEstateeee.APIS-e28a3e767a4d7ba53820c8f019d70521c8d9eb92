
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using Quarter.APIS.Helper;
using Quarter.Core;
using Quarter.Core.Mapping.Estates;
using Quarter.Core.ServiceContract;
using Quarter.Repostory;
using Quarter.Repostory.Data;
using Quarter.Repostory.Data.Context;
using Quarter.Service.Service;
using Quarter.Service.Service.Estates;
using Store.APIS.Helper;

namespace QuarterEstate.APIS
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDependency(builder.Configuration);
            builder.Services.AddScoped<IBlobService, BlobService>();


            var app = builder.Build();
            await app.ConfigureMiddlewareAsync();

            app.Run();
        }
    }
}

