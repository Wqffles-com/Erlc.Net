using Erlc.Net.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Erlc.Net.Extensions.Hosting;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddErlcClientPool(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ErlcOptions>(configuration);
        return AddErlcClientPoolInternal(services);
    }

    public static IServiceCollection AddErlcClientPool(this IServiceCollection services, string apiKey)
    {
        services.Configure<ErlcOptions>(options => { });
        return AddErlcClientPoolInternal(services, apiKey);
    }

    public static IServiceCollection AddErlcClientPool(this IServiceCollection services, Action<ErlcOptions> configureOptions)
    {
        services.Configure(configureOptions);
        return AddErlcClientPoolInternal(services);
    }

    private static IServiceCollection AddErlcClientPoolInternal(IServiceCollection services, string? initialApiKey = null)
    {
        services.AddHttpClient(nameof(ErlcClient));

        services.AddSingleton<ErlcClientPool>(serviceProvider =>
        {
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var options = serviceProvider.GetRequiredService<IOptions<ErlcOptions>>().Value;

            var pool = new ErlcClientPool(httpClientFactory, options.EnablePollingByDefault);

            if (!string.IsNullOrEmpty(initialApiKey))
            {
                pool.GetOrAddClient("default", initialApiKey, options.DefaultPollingIntervalSeconds);
            }

            return pool;
        });

        return services;
    }

    public static IServiceCollection AddErlcClient(this IServiceCollection services, IConfiguration configuration)
        => AddErlcClientPool(services, configuration);

    public static IServiceCollection AddErlcClient(this IServiceCollection services, string apiKey)
        => AddErlcClientPool(services, apiKey);

    public static IServiceCollection AddErlcClient(this IServiceCollection services, Action<ErlcOptions> configureOptions)
        => AddErlcClientPool(services, configureOptions);
}
