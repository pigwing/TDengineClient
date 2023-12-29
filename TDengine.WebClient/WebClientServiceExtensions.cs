using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;
using Refit;

namespace TDengine.WebClient
{
    public static class WebClientServiceExtensions
    {
        public static IServiceCollection AddTDengineWebClient(this IServiceCollection services, Action<ConnectionConfiguration> action, IList<ConnectionConfiguration>? connectionConfigurations = null)
        {
            ConnectionConfiguration configuration = new ConnectionConfiguration();
            services.AddSingleton(configuration);
            services.AddTransient<AuthHeaderHandler>();
            action(configuration);
            if (string.IsNullOrEmpty(configuration.Database))
                throw new InvalidOperationException("Database is null");

            services.AddRefitClient<ITDengineRestApi>().ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration.Host ?? throw new InvalidOperationException("Host is null"));
            }).AddHttpMessageHandler<AuthHeaderHandler>();

            if (connectionConfigurations is { Count: > 0 })
            {
                Dictionary<string, ConnectionConfiguration> connectionConfigurationDic =
                    new Dictionary<string, ConnectionConfiguration>();
                foreach (ConnectionConfiguration connectionConfiguration in connectionConfigurations)
                {
                    if (!string.IsNullOrEmpty(connectionConfiguration.Host))
                    {
                        connectionConfigurationDic.TryAdd(connectionConfiguration.Host, connectionConfiguration);
                    }
                }

                services.AddSingleton(connectionConfigurationDic);
            }

            services.AddHttpClient<ITDengineHostRestApi>("__default__").AddHttpMessageHandler<AuthHeaderHandler>();

            services.AddTransient<TDengineQuery>();
            return services;
        }
    }
}
