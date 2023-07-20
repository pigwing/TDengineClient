using System;
using System.Text.Json.Serialization;

namespace TDengineAspCoreClient
{
    [JsonSerializable(typeof(List<HistoryData>))]
    [JsonSerializable(typeof(HistoryData), GenerationMode = JsonSourceGenerationMode.Metadata)]
    [JsonSourceGenerationOptions(WriteIndented = true, GenerationMode = JsonSourceGenerationMode.Serialization)]
    internal partial class MyJsonContext : JsonSerializerContext
    {
    }
}
