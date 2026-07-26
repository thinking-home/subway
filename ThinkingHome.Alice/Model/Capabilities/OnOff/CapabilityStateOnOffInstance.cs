using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.OnOff;

/// <summary>Инстанс способности on_off.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CapabilityStateOnOffInstance
{
    /// <summary>Включение/выключение — единственный инстанс on_off.</summary>
    [JsonStringEnumMemberName("on")] On,
}