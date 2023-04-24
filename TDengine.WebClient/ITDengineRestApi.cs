using WebApiClientCore.Attributes;

namespace TDengine.WebClient
{

    [LoggingFilter]
    public interface ITDengineRestApi
    {
        [HttpPost("rest/sql/{database}")]
        [JsonNetReturn]
        public Task<TDengineResponse> ExecuteQueryAsync(string database, [RawStringContent("txt/plain")] string sql);
        [HttpPost("rest/sql")]
        [JsonNetReturn]
        public Task<TDengineResponse> ExecuteQueryAsync([RawStringContent("txt/plain")] string sql);
    }
}