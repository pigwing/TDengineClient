using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDengine.WebClient
{
    public class TDengineQuery
    {
        private readonly ITDengineRestApi _tDengineRestApi;
        private readonly ConnectionConfiguration _connectionConfiguration;

        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        public TDengineQuery(ITDengineRestApi tDengineRestApi, ConnectionConfiguration connectionConfiguration)
        {
            _tDengineRestApi = tDengineRestApi;
            _connectionConfiguration = connectionConfiguration;
        }

        public async Task<TDengineResponse> RawQueryAsync(string sql)
        {
            TDengineResponse tdDengineResponse = await _tDengineRestApi.ExecuteQueryAsync(_connectionConfiguration.Database!, sql);
            return tdDengineResponse;
        }

        private string ConvertToJsonString(TDengineResponse tdDengineResponse)
        {
            StringBuilder sb = new StringBuilder();
            if (tdDengineResponse.Rows > 1)
                sb.Append("[");

            for (int j = 0; j < tdDengineResponse.Data!.Count; j++)
            {
                object[] row = tdDengineResponse.Data![j];
                sb.Append("{");
                for (int i = 0; i < tdDengineResponse.ColumnTDengineMeta!.Count; i++)
                {
                    string jsonName = tdDengineResponse.ColumnTDengineMeta[i].Name!;
                    TDengineDataType tDengineDataType = tdDengineResponse.ColumnTDengineMeta[i].DataType;
                    object targetValue = row[i];

                    sb.Append(@"""").Append(jsonName).Append(@"""").Append(":");

                    switch (tDengineDataType)
                    {
                        case TDengineDataType.Timestamp:
                        {
                            DateTime.TryParse(targetValue.ToString(), out DateTime utcDateTime);
                            DateTime localTime = utcDateTime.ToLocalTime();

                            sb.Append(@"""").Append(localTime).Append(@"""");
                        }
                            break;
                        case TDengineDataType.Binary:
                        case TDengineDataType.NChart:
                        case TDengineDataType.VarChar:
                        {
                            sb.Append(@"""").Append(targetValue).Append(@"""");
                        }
                            break;
                        case TDengineDataType.Bool:
                        {
                            sb.Append(targetValue.ToString()!.ToLower());
                        }
                            break;
                        default:
                            sb.Append(targetValue);
                            break;
                    }

                    if (i != tdDengineResponse.ColumnTDengineMeta!.Count - 1)
                        sb.Append(",");
                }

                sb.Append("}");

                if (j != tdDengineResponse.Data!.Count - 1)
                    sb.Append(",");
            }


            if (tdDengineResponse.Rows > 1)
                sb.Append("]");

            string jsonString = sb.ToString();

            return jsonString;
        }

        public async Task<T?> QueryAsync<T>(string sql) where T : class, new()
        {
            TDengineResponse tdDengineResponse =
                await _tDengineRestApi.ExecuteQueryAsync(_connectionConfiguration.Database!, sql);
            if (tdDengineResponse.Code != 0)
                throw new ArgumentException(tdDengineResponse.Description);

            string jsonString = ConvertToJsonString(tdDengineResponse);
            return JsonConvert.DeserializeObject<T>(jsonString);
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
        public async Task<T?> QueryAsync<T>(string sql, ICollection<TDengineParameter> parameters) where T : class, new()
        {
            string sqlRewrite = SqlGenerator(sql, parameters);
            return await QueryAsync<T>(sqlRewrite);
        }


    }
}
