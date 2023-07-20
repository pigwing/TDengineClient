using TDengine.WebClient.Attribute;
using WebApiClientCore.Attributes;

namespace TDengine.WebClient
{

    //[LoggingFilter]
    public interface ITDengineRestApi
    {
        [HttpPost("rest/sql/{database}")]
        [JsonReturn]
        [DynamicHost]
        public Task<TDengineResponse> ExecuteQueryAsync([Uri] string host, string database, [RawStringContent("txt/plain")] string sql);
        [HttpPost("rest/sql/{database}")]
        [JsonReturn]
        public Task<TDengineResponse> ExecuteQueryAsync(string database, [RawStringContent("txt/plain")] string sql);
        [HttpPost("rest/sql")]
        [JsonReturn]
        public Task<TDengineResponse> ExecuteQueryAsync([RawStringContent("txt/plain")] string sql);
    }
}