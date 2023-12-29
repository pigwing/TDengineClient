using System.Text;

namespace TDengine.WebClient
{
    internal class AuthHeaderHandler(ConnectionConfiguration connectionConfiguration) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string auth = $"{connectionConfiguration.Username}:{connectionConfiguration.Password}";
            auth = $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes(auth))}";
            request.Headers.Add("Authorization", auth);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
