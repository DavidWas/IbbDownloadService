using System.Reflection;
using IbbDownloadService.NugetModule.Contracts.Events;
using IbbDownloadService.NugetModule.Entities;
using IbbDownloadService.NugetModule.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace IbbDownloadService.NugetModule;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNugetModule(this IServiceCollection services, IConfigurationManager config,
        ILogger logger, List<Assembly> mediatRAssemblies)
    {
        services.AddDbContext<NugetDbContext>(options => { options.UseSqlite("Data Source=nuget.db"); });
        mediatRAssemblies.Add(typeof(ServiceCollectionExtensions).Assembly);
        services.AddSingleton<NugetUpdatedEvent>();
        services.AddHostedService<NugetVersionSync>();
        logger.Information("Finished adding NugetModule services");
        return services;
    }

    public static IApplicationBuilder UseNugetModule(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<NugetDbContext>();
        dbContext.Database.Migrate();
        return app;
    }
}