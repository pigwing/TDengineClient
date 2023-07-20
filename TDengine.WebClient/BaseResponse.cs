namespace TDengine.WebClient
{
    public abstract class BaseResponse
    {
        public int Code { get; set; }
        public int Rows { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("desc")]
        public string? Description { get; set; }
    }
}
