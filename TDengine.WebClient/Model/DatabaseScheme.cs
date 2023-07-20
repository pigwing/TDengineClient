namespace TDengine.WebClient.Model
{
    public class DatabaseScheme : BaseResponse
    {
        public string? Name { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("create_time")]
        public DateTime? CreateTime { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("vgroups")]
        public int VGroups { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("ntables")]
        public int NTables { get; set; }
        public byte Replica { get; set; }
        public string? Strict { get; set; }
        public string? Duration { get; set; }
        public string? Keep { get; set; }
        public int Buffer { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("pagesize")]
        public int PageSize { get; set; }
        public int Pages { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("minrows")]
        public int MinRows { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("maxrows")]
        public int MaxRows { get; set; }
        public byte Comp { get; set; }
        public string? Precision { get; set; }
        public string? Status { get; set; }
        public string? Retentions { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("single_stable")]
        public bool SingleStable { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("cachemodel")]
        public string? CacheModel { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("cachesize")]
        public int CacheSize { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("wal_level")]
        public byte WalLevel { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("wal_fsync_period")]
        public int WalFSyncPeriod { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("wal_retention_period")]
        public int WalRetentionPeriod { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("wal_retention_size")]
        public long WalRetentionSize { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("wal_roll_period")]
        public int WalRollPeriod { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("wal_segment_size")]
        public long WalSegmentSize { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("stt_trigger")]
        public short SttTrigger { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("table_prefix")]
        public short TablePrefix { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("JsonProperty")]
        public short TableSuffix { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("tsdb_pagesize")]
        public int TsDbPageSize { get; set; }
    }
}
