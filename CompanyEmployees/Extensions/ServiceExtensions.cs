using Microsoft.AspNetCore.Cors.Infrastructure;

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
}
