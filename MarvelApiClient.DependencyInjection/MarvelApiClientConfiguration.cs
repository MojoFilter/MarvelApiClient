using MarvelApiClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public class MarvelApiOptions
{
    public required ApiKey ApiKey { get; set; }
    public required PrivateKey PrivateKey { get; set; }
}
public static class MarvelApiClientConfiguration
{
    public static IServiceCollection AddMarvelApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MarvelApiOptions>(configuration);
        services.AddScoped<IMarvelApiClientBuilder, MarvelApiClientBuilder>();
        services.AddScoped(BuildClient);
        return services;
    }

    private static IMarvelApiClient BuildClient(IServiceProvider provider)
    {
        var options = provider.GetRequiredService<IOptions<MarvelApiOptions>>().Value;
        var builder = provider.GetRequiredService<IMarvelApiClientBuilder>();
        return builder.Build(options.ApiKey, options.PrivateKey);
    }
}
