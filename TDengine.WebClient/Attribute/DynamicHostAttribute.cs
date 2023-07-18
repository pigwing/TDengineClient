using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace TDengine.WebClient.Attribute
{
    public class DynamicHostAttribute : ApiActionAttribute
    {

        public override Task OnRequestAsync(ApiRequestContext context)
        {
            Dictionary<string, ConnectionConfiguration>? connectionConfigurations = context.HttpContext.ServiceProvider
                .GetService<Dictionary<string, ConnectionConfiguration>>();

            if (connectionConfigurations != null)
            {
                if (connectionConfigurations.TryGetValue(context.Arguments[0]!.ToString() ?? throw new ArgumentNullException($"host cannot be null"),
                        out ConnectionConfiguration? connectionConfiguration))
                {
                    
                    string auth = $"{connectionConfiguration.Username}:{connectionConfiguration.Password}";
                    auth = $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes(auth))}";
                    string key = "Authorization";
                    context.HttpContext.HttpClient.DefaultRequestHeaders.Remove(key);
                    context.HttpContext.HttpClient.DefaultRequestHeaders.Add(key, auth);
                }
            }
            return Task.CompletedTask;
        }
    }
}
