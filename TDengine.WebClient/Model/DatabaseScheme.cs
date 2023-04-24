using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApiClientCore.Attributes;

namespace TDengine.WebClient.Model
{
    public class DatabaseScheme : BaseResponse
    {
        public string? Name { get; set; }
        [JsonProperty(PropertyName = "create_time")]
        public DateTime? CreateTime { get; set; }
        [JsonProperty(PropertyName = "vgroups")]
        public int VGroups { get; set; }
        [JsonProperty(PropertyName = "ntables")]
        public int NTables { get; set; }
        public byte Replica { get; set; }
        public string? Strict { get; set; }
        public string? Duration { get; set; }
        public string? Keep { get; set; }
        public int Buffer { get; set; }
        [JsonProperty(PropertyName = "pagesize")]
        public int PageSize { get; set; }
        public int Pages { get; set; }
        [JsonProperty(PropertyName = "minrows")]
        public int MinRows { get; set; }
        [JsonProperty(PropertyName = "maxrows")]
        public int MaxRows { get; set; }
        public byte Comp { get; set; }
        public string? Precision { get; set; }
        public string? Status { get; set; }
        public string? Retentions { get; set; }
        [JsonProperty(PropertyName = "single_stable")]
        public bool SingleStable { get; set; }
        [JsonProperty(PropertyName = "cachemodel")]
        public string? CacheModel { get; set; }
        [JsonProperty(PropertyName = "cachesize")]
        public int CacheSize { get; set; }
        [JsonProperty(PropertyName = "wal_level")]
        public byte WalLevel { get; set; }
        [JsonProperty(PropertyName = "wal_fsync_period")]
        public int WalFSyncPeriod { get; set; }
        [JsonProperty(PropertyName = "wal_retention_period")]
        public int WalRetentionPeriod { get; set; }
        [JsonProperty(PropertyName = "wal_retention_size")]
        public long WalRetentionSize { get; set; }
        [JsonProperty(PropertyName = "wal_roll_period")]
        public int WalRollPeriod { get; set; }
        [JsonProperty(PropertyName = "wal_segment_size")]
        public long WalSegmentSize { get; set; }
        [JsonProperty(PropertyName = "stt_trigger")]
        public short SttTrigger { get; set; }
        [JsonProperty(PropertyName = "table_prefix")]
        public short TablePrefix { get; set; }
        [JsonProperty(PropertyName = "JsonProperty")]
        public short TableSuffix { get; set; }
        [JsonProperty(PropertyName = "tsdb_pagesize")]
        public int TsDbPageSize { get; set; }
    }
}
