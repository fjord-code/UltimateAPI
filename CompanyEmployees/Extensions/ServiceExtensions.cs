using Contracts;
using LoggerService;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service;
using Service.Contracts;

namespace CompanyEmployees.Extensions;

/// <summary>
/// Contains extension methods to configure web application services.
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Configures the CORS policy to allow requests from any sources.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection ConfigureCORS(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }

    /// <summary>
    /// Configures an IIS integration.
    /// </summary>
    /// <param name="services">Service collection.</param>
    public static void ConfigureIISIntegration(this IServiceCollection services)
    {
        services.Configure<IISOptions>(options => { });
    }

    /// <summary>
    /// Configures the logger.
    /// </summary>
    /// <param name="services">Service collection.</param>
    public static void ConfigureLoggerService(this IServiceCollection services) 
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    public static void ConfigureServiceManager(this IServiceCollection services) =>
        services.AddScoped<IServiceManager, ServiceManager>();

    public static void ConfigureSqlContext(this IServiceCollection services) =>
        services.AddDbContext<RepositoryContext>(options =>
            options.UseSqlite(Environment.GetEnvironmentVariable("SqliteConnectionString")));

    public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder) =>
        builder.AddMvcOptions(config => config.OutputFormatters.Add(new CsvOutputFormatter()));
}
