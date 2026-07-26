using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.OnOff;

/// <summary>Состояние on_off: инстанс и логическое значение.</summary>
public class CapabilityStateOnOffData
{
    /// <summary>Инстанс способности (единственный — "on").</summary>
    [JsonPropertyName("instance")] public CapabilityStateOnOffInstance Instance { get; set; }

    /// <summary>Включено ли устройство.</summary>
    [JsonPropertyName("value")] public bool Value { get; set; }
}