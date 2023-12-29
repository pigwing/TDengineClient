using Refit;

namespace TDengine.WebClient
{

    //[LoggingFilter]
    public interface ITDengineRestApi
    {
        [Headers("Content-Type: txt/plain")]
        [Post("/rest/sql/{database}")]
        public Task<TDengineResponse> ExecuteQueryAsync(string database, [Body] string sql);
        [Headers("Content-Type: txt/plain")]
        [Post("/rest/sql")]
        public Task<TDengineResponse> ExecuteQueryAsync([Body] string sql);
    }

    public interface ITDengineHostRestApi
    {
        [Headers("Content-Type: txt/plain")]
        [Post("/rest/sql/{database}")]
        public Task<TDengineResponse> ExecuteQueryAsync(string host, string database, [Body] string sql);
    }
}