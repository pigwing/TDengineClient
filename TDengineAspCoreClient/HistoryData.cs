using Newtonsoft.Json;

namespace TDengineAspCoreClient
{
    public class HistoryData
    {
        public long UnitId { get; set; }
        public string? Alias { get; set; }
        public DateTime RecordTime { get; set; }
        [JsonProperty(PropertyName = "val")]
        public double Value { get; set; }
        public bool SoftDelete { get; set; }
    }
}
