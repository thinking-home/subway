using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Range;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CapabilityStateRangeInstance
{
    [JsonStringEnumMemberName("brightness")] Brightness,
    [JsonStringEnumMemberName("open")] Open,
}
