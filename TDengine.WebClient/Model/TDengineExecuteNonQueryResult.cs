namespace TDengine.WebClient.Model
{
    public class TDengineExecuteNonQueryResult : BaseResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("affected_rows")]
        public int AffectedRows { get; set; }
    }
}
