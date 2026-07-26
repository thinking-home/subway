using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Mode;

/// <summary>Инстанс способности mode — какая режимная функция устройства управляется.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CapabilityModeInstance
{
    /// <summary>Скорость вентилятора.</summary>
    [JsonStringEnumMemberName("fan_speed")] FanSpeed,
    /// <summary>Режим работы термостата.</summary>
    [JsonStringEnumMemberName("thermostat")] Thermostat,
}
