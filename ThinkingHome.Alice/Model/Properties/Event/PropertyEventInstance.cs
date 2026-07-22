using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Properties.Event;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PropertyEventInstance
{
    [JsonStringEnumMemberName("motion")] Motion,
    [JsonStringEnumMemberName("open")] Open,
}
