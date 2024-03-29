﻿using System.Buffers;
using Cysharp.Text;
using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Refit;

namespace TDengine.WebClient
{
    public class TDengineQuery
    {
        private readonly ITDengineRestApi _tDengineRestApi;
        private readonly ConnectionConfiguration _connectionConfiguration;
        private readonly IHttpClientFactory _httpClientFactory;

        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        private const string DateTimeSystemTextJsonFormat = "yyyy-MM-ddTHH:mm:ss";
        private readonly JsonSerializerOptions _jsonSerializerOptions;


        public TDengineQuery(ITDengineRestApi tDengineRestApi, ConnectionConfiguration connectionConfiguration, IHttpClientFactory httpClientFactory)
        {
            _tDengineRestApi = tDengineRestApi;
            _connectionConfiguration = connectionConfiguration;
            _httpClientFactory = httpClientFactory;
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<TDengineResponse> RawQueryAsync(string sql)
        {
            TDengineResponse tdDengineResponse = await _tDengineRestApi.ExecuteQueryAsync(_connectionConfiguration.Database!, sql);
            return tdDengineResponse;
        }


        private ReadOnlySpan<char> ConvertToJsonString(TDengineResponse tdDengineResponse)
        {
            using var writer = ZString.CreateStringBuilder(true);

            if (tdDengineResponse.Rows >= 1)
                writer.Append("[");


            for (int j = 0; j < tdDengineResponse.Data!.Count; j++)
            {
                object[] row = tdDengineResponse.Data[j];
                writer.Append("{");
                for (int i = 0; i < tdDengineResponse.ColumnTDengineMeta!.Count; i++)
                {
                    string jsonName = tdDengineResponse.ColumnTDengineMeta[i].Name!;
                    TDengineDataType tDengineDataType = tdDengineResponse.ColumnTDengineMeta[i].DataType;
                    object targetValue = row[i];
                    writer.Append(@"""");
                    writer.Append(jsonName);
                    writer.Append(@"""");
                    writer.Append(":");

                    switch (tDengineDataType)
                    {
                        case TDengineDataType.Timestamp:
                        {
                            DateTime.TryParse(targetValue.ToString(), out DateTime utcDateTime);
                            DateTime localTime = utcDateTime.ToLocalTime();

                            writer.Append(@"""");
                            writer.Append(localTime.ToString(DateTimeSystemTextJsonFormat)); 
                            writer.Append(@"""");
                        }
                            break;
                        case TDengineDataType.Binary:
                        case TDengineDataType.NChart:
                        case TDengineDataType.VarChar:
                        {
                            writer.Append(@"""");
                            writer.Append(targetValue == null ? "" : targetValue.ToString()); 
                            writer.Append(@"""");
                        }
                            break;
                        case TDengineDataType.Bool:
                        {
                            writer.Append(targetValue == null ? "false" : targetValue.ToString()!.ToLower());
                        }
                            break;
                        case TDengineDataType.BigInt:
                        case TDengineDataType.BigIntUnsigned:
                        case TDengineDataType.Int:
                        case TDengineDataType.IntUnsigned:
                        case TDengineDataType.Float:
                        case TDengineDataType.Double:
                        case TDengineDataType.SmallInt:
                        case TDengineDataType.SmallIntUnsigned:
                        case TDengineDataType.TinyInt:
                        case TDengineDataType.TinyIntUnsigned:
                        {
                            writer.Append(targetValue == null ? "0" : targetValue.ToString());
                        }
                            break;
                        default:
                            writer.Append(targetValue == null ? "null" : targetValue.ToString());
                            break;
                    }

                    if (i != tdDengineResponse.ColumnTDengineMeta!.Count - 1)
                        writer.Append(",");
                }

                writer.Append("}");

                if (j != tdDengineResponse.Data!.Count - 1)
                    writer.Append(",");
            }

            if (tdDengineResponse.Rows >= 1)
                writer.Append("]");

            var jsonString = writer.AsSpan();
            //Debug.Print(jsonString.ToString());
            return jsonString;
        }

        private T? GetObject<T>(ReadOnlySpan<char> jsonString, JsonTypeInfo<T>? jsonTypeInfo = null)
        {
            if (jsonTypeInfo != null)
            {
                return JsonSerializer.Deserialize<T>(jsonString, jsonTypeInfo);
            }
            return JsonSerializer.Deserialize<T>(jsonString, _jsonSerializerOptions);
        }

        private T? GetResult<T>(TDengineResponse tdDengineResponse, JsonTypeInfo<T>? jsonTypeInfo = null)
        {
            if (tdDengineResponse.Code != 0)
                throw new ArgumentException(tdDengineResponse.Description);
            return tdDengineResponse.Rows == 0 ? default : GetObject(ConvertToJsonString(tdDengineResponse), jsonTypeInfo);
        }

        public async Task<T?> QueryAsync<T>(string sql, JsonTypeInfo<T>? jsonTypeInfo = null) where T : IEnumerable
        {
            TDengineResponse tdDengineResponse =
                await _tDengineRestApi.ExecuteQueryAsync(_connectionConfiguration.Database!, sql);

            return GetResult(tdDengineResponse, jsonTypeInfo);
        }

        public async Task<T?> QueryAsync<T>(string database, string sql, JsonTypeInfo<T>? jsonTypeInfo = null) where T : IEnumerable
        {
            TDengineResponse tdDengineResponse =
                await _tDengineRestApi.ExecuteQueryAsync(database, sql);

            return GetResult(tdDengineResponse, jsonTypeInfo);
        }

        public async Task<T?> QueryAsync<T>(string host, string database, string sql, JsonTypeInfo<T>? jsonTypeInfo = null) where T : IEnumerable
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("__default__");
            httpClient.BaseAddress = new Uri(host);

            ITDengineHostRestApi hostRestApi = RestService.For<ITDengineHostRestApi>(httpClient);
            TDengineResponse tdDengineResponse =
                await hostRestApi.ExecuteQueryAsync(host, database, sql);

            return GetResult(tdDengineResponse, jsonTypeInfo);
        }



        private string SqlGenerator(string rawSql, ICollection<TDengineParameter> parameters)
        {
            string sql = rawSql;
            foreach (TDengineParameter parameter in parameters)
            {
                switch (parameter.DataType)
                {
                    case TDengineDataType.Binary:
                    case TDengineDataType.NChart:
                    case TDengineDataType.VarChar:
                        sql = sql.Replace($"@{parameter.Name}", $"'{parameter.Value}'");
                        break;
                    case TDengineDataType.Bool:
                        bool boolValue = false;
                        if (parameter.Value != null)
                        {
                            boolValue = (bool)parameter.Value;
                        }
                        sql = sql.Replace($"@{parameter.Name}", $"{boolValue.ToString().ToLower()}");
                        break;
                    case TDengineDataType.Json:
                        break;
                    case TDengineDataType.Timestamp:
                    {
                        DateTime recordTime = default(DateTime);
                        if (parameter.Value != null)
                        {
                            DateTime.TryParse(parameter.Value.ToString(), out recordTime);
                        }
                        sql = sql.Replace($"@{parameter.Name}", $"'{recordTime.ToString(DateTimeFormat)}'");
                    }
                        break;
                    case TDengineDataType.Float:
                    {
                        float floatValue = default(float);
                        if (parameter.Value != null)
                        {
                            float.TryParse(parameter.Value.ToString(), out floatValue);
                        }
                        sql = sql.Replace($"@{parameter.Name}", $"{floatValue}");
                        break;
                    }
                    case TDengineDataType.Double:
                    {
                        double doubleValue = default(double);
                        if (parameter.Value != null)
                        {
                            double.TryParse(parameter.Value.ToString(), out doubleValue);
                        }
                        sql = sql.Replace($"@{parameter.Name}", $"{doubleValue}");
                    }
                        break;
                    default:
                        long val = 0;
                        if (parameter.Value != null)
                        {
                            long.TryParse(parameter.Value.ToString(), out val);
                        }
                        sql = sql.Replace($"@{parameter.Name}", $"{val}");
                        break;
                }
                
            }

            return sql;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters">使用@开始作为占位符</param>
        /// <returns></returns>
        public async Task<T?> QueryAsync<T>(string sql, ICollection<TDengineParameter> parameters) where T : IEnumerable
        {
            string sqlRewrite = SqlGenerator(sql, parameters);
            return await QueryAsync<T>(sqlRewrite);
        }


    }
}
