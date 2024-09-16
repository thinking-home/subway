using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.OnOff;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CapabilityStateOnOffInstance
{
    [JsonStringEnumMemberName("on")] On,
}