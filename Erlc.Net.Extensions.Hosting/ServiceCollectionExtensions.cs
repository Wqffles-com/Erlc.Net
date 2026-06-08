using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Erlc.Net.Extensions.Hosting;

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
        services.AddHttpClient(nameof(Erlc.Net.Core.ErlcClient));

        services.AddTransient<Erlc.Net.Core.ErlcClient>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<ErlcOptions>>().Value;
            var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(Erlc.Net.Core.ErlcClient));
            return new Erlc.Net.Core.ErlcClient(options.ApiKey, httpClient);
        });

        return services;
    }
}
