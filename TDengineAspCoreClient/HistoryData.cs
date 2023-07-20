namespace TDengineAspCoreClient
{
    public class HistoryData
    {
        [System.Text.Json.Serialization.JsonPropertyName("unitid")]
        public long UnitId { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("alias")]
        public string? Alias { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("recordtime")]
        public DateTime RecordTime { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("val")]
        public double Value { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("softdelete")]
        public bool SoftDelete { get; set; }
    }
}
