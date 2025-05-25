

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quarter.Core.Entities.Identity;
using Quarter.Repository.Identity;
using Quarter.Repostory;
using Quarter.Repostory.Data.Context;
using Quarter.Repostory.Identity.Contexts;
using QuarterEstate.APIS;
using QuarterEstate.APIS.Middleware;

namespace Quarter.APIS.Helper
{
    public static class ConfigureMiddelware
    {
        public static async Task<IApplicationBuilder> ConfigureMiddlewareAsync(this WebApplication app)
        {
            using var scope=app.Services.CreateScope();
            var service = scope.ServiceProvider;
            var context = service.GetRequiredService<QuarterDbContexts>();
                   var LoggerFactory = service.GetRequiredService<ILoggerFactory>();
            var identitycontext = service.GetRequiredService<StoreIdentityDbContext>();
            var UserManager = service.GetRequiredService<UserManager<AppUser>>();

            try
            {
                await context.Database.MigrateAsync();
                await StoreDbContextSeed.SeedAsync(context);
                await identitycontext.Database.MigrateAsync();
                await StoreIdentityDbContextSeed.SeedAppUserAsync(UserManager);

            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "في مشاكل ياصحبي");
            }
            app.UseMiddleware<ExceptionMiddleware>();
            // Configure the HTTP request pipeline.

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            return app;


        }

    }
}
