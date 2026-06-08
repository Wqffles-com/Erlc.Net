using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Extensions.Hosting;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddErlcClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ErlcOptions>(configuration);
        return AddErlcClientInternal(services);
    }
    public static IServiceCollection AddErlcClient(this IServiceCollection services, string apiKey)
    {
        services.Configure<ErlcOptions>(options => options.ApiKey = apiKey);
        return AddErlcClientInternal(services);
    }

    public static IServiceCollection AddErlcClient(this IServiceCollection services, Action<ErlcOptions> configureOptions)
    {
        services.Configure(configureOptions);
        return AddErlcClientInternal(services);
    }

    private static IServiceCollection AddErlcClientInternal(IServiceCollection services)
    {
        services.AddHttpClient(nameof(Core.ErlcClient));

        services.AddTransient<Core.ErlcClient>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<ErlcOptions>>().Value;
            var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(Core.ErlcClient));
            return new Core.ErlcClient(options.ApiKey, httpClient);
        });

        return services;
    }
}
