using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;

namespace TDengine.WebClient
{
    public static class WebClientServiceExtensions
    {
        public static IServiceCollection AddTDengineWebClient(this IServiceCollection services, Action<ConnectionConfiguration> action, IList<ConnectionConfiguration>? connectionConfigurations = null)
        {
            ConnectionConfiguration configuration = new ConnectionConfiguration();
            services.AddSingleton(configuration);
            action(configuration);
            if (string.IsNullOrEmpty(configuration.Database))
                throw new InvalidOperationException("Database is null");
            services.AddHttpApi<ITDengineRestApi>(option =>
            {
                option.HttpHost = new Uri(configuration.Host ?? throw new InvalidOperationException("Host is null"));
            }).ConfigureHttpClient(httpClient =>
            {
                string auth = $"{configuration.Username}:{configuration.Password}";
                auth = $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes(auth))}";
                httpClient.DefaultRequestHeaders.Add("Authorization", auth);
            });

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

            services.AddTransient<TDengineQuery>();
            return services;
        }
    }
}
