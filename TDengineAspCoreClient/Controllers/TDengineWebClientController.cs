using Microsoft.AspNetCore.Mvc;
using System.Text;
using TDengine.WebClient;
using TDengine.WebClient.Model;

namespace TDengineAspCoreClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TDengineWebClientController : ControllerBase
    {
       
        private readonly TDengineQuery _tDengineQuery;
        private readonly ITDengineRestApi _tDengineRestApi;
        private readonly ConnectionConfiguration _connectionConfiguration;

        public TDengineWebClientController(TDengineQuery tDengineQuery, ITDengineRestApi tDengineRestApi, ConnectionConfiguration connectionConfiguration)
        {
            _tDengineQuery = tDengineQuery;
            _tDengineRestApi = tDengineRestApi;
            _connectionConfiguration = connectionConfiguration;
        }

        [HttpGet("CreateDb")]
        public async Task<TDengineResponse?> CreateDbAsync()
        {
            TDengineResponse response = await _tDengineRestApi.ExecuteQueryAsync($"CREATE DATABASE {_connectionConfiguration.Database} KEEP 3650");
            return response;
        }

        [HttpGet("CreateTable")]
        public async Task<TDengineResponse?> CreateTableAsync()
        {
            TDengineResponse response = await _tDengineQuery.RawQueryAsync(
                "CREATE STABLE historydata (recordtime timestamp, val double, softdelete bool) TAGS (unitid bigint, alias binary(255))");
            return response;
        }

        [HttpGet("RawSql")]
        public async Task<TDengineResponse?> RawSqlAsync(string sql)
        {
            TDengineResponse response = await _tDengineQuery.RawQueryAsync(sql);
            return response;
        }

        [HttpGet("DatabaseShceme")]
        public async Task<DatabaseScheme?> QueryDatabaseSchemeAsync(string databaseName)
        {
            string sql = $@"SELECT * FROM INFORMATION_SCHEMA.INS_DATABASES WHERE NAME='{databaseName}'";
            DatabaseScheme? databaseScheme = await _tDengineQuery.QueryAsync<DatabaseScheme>(sql);
            return databaseScheme;
        }

        [HttpGet("QueryHistoryDataByHost")]
        public async Task<List<HistoryData>?> QuerySqlAsync(string host, string database, string sql)
        {
            List<HistoryData>? response = await _tDengineQuery.QueryAsync<List<HistoryData>>(host, database, sql);
            return response;
        }

        [HttpGet("QueryHistoryData")]
        public async Task<List<HistoryData>?> QuerySqlAsync(string sql)
        {
            List<HistoryData>? response = await _tDengineQuery.QueryAsync<List<HistoryData>>(sql);
            return response;
        }

        [HttpGet("AddTestData")]
        public async Task<TDengineExecuteNonQueryResult?> AddTestData(long insertCount, long unitId, string alias)
        {
            DateTime now = DateTime.Now;
            Random random = new Random((int)DateTime.Now.Ticks);
            List<HistoryData> historyDatas = new List<HistoryData>();
            for (int i = 0; i < insertCount; i++)
            {
                DateTime recordTime = now.AddSeconds(i);
                historyDatas.Add(new HistoryData()
                {
                    Alias = alias,
                    UnitId = unitId,
                    RecordTime = recordTime,
                    SoftDelete = false,
                    Value = random.NextDouble()
                });
            }

            StringBuilder sb = new StringBuilder("INSERT INTO ");
            foreach (HistoryData addHistoryDataFormModel in historyDatas)
            {
                string tableNameMD5 =
                    TDengineSuperTable.GetTableName(addHistoryDataFormModel.UnitId, addHistoryDataFormModel.Alias)
                        .CreateMD5();
                string singleSql =
                    $"t{tableNameMD5} USING HistoryData TAGS({addHistoryDataFormModel.UnitId}, '{addHistoryDataFormModel.Alias}') VALUES ('{addHistoryDataFormModel.RecordTime.ToString("yyyy-MM-dd HH:mm:ss")}', {addHistoryDataFormModel.Value}, '{addHistoryDataFormModel.SoftDelete.ToString().ToLower()}') ";
                sb.Append(singleSql);
            }
            string sql = sb.ToString();

            TDengineExecuteNonQueryResult? response =  await _tDengineQuery.QueryAsync<TDengineExecuteNonQueryResult>(sql);

            return response;
        }

        [HttpPost("Update")]
        public async Task<TDengineExecuteNonQueryResult?> UpdateAsync([FromBody]HistoryData historyData)
        {
            string tableNameMD5 =
                TDengineSuperTable.GetTableName(historyData.UnitId, historyData.Alias)
                    .CreateMD5();
            string table = $"t{tableNameMD5}";
            string sql =$"INSERT INTO {table} USING HistoryData TAGS({historyData.UnitId}, '{historyData.Alias}') VALUES ('{historyData.RecordTime.ToString("yyyy-MM-dd HH:mm:ss")}', {historyData.Value}, '{historyData.SoftDelete.ToString().ToLower()}')";
            TDengineExecuteNonQueryResult? result = await _tDengineQuery.QueryAsync<TDengineExecuteNonQueryResult>(sql, new List<TDengineParameter>()
            {
                new TDengineParameter()
                {
                    Name = "recordtime",
                    DataType = TDengineDataType.Timestamp,
                    Value = historyData.RecordTime
                },
                new TDengineParameter()
                {
                    Name = "val",
                    DataType = TDengineDataType.Double,
                    Value = historyData.Value
                }
            });

            return result;
        }

        [HttpPost("Delete")]
        public async Task<TDengineExecuteNonQueryResult?> DeleteAsync([FromBody] HistoryData historyData)
        {
            string tableNameMD5 =
                TDengineSuperTable.GetTableName(historyData.UnitId, historyData.Alias)
                    .CreateMD5();
            string table = $"t{tableNameMD5}";
            string sql = $"DELETE FROM {table} WHERE recordtime = @recordtime";
            TDengineExecuteNonQueryResult? result = await _tDengineQuery.QueryAsync<TDengineExecuteNonQueryResult>(sql, new List<TDengineParameter>()
            {
                new TDengineParameter()
                {
                    Name = "recordtime",
                    DataType = TDengineDataType.Timestamp,
                    Value = historyData.RecordTime
                }
            });

            return result;
        }
    }
}
