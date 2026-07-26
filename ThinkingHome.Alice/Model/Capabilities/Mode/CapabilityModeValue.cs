using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Mode;

/// <summary>Значения режима из фиксированного словаря Алисы. Пока — наборы для fan_speed и thermostat.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CapabilityModeValue
{
    /// <summary>Автоматический режим.</summary>
    [JsonStringEnumMemberName("auto")] Auto,
    /// <summary>Низкая скорость.</summary>
    [JsonStringEnumMemberName("low")] Low,
    /// <summary>Средняя скорость.</summary>
    [JsonStringEnumMemberName("medium")] Medium,
    /// <summary>Высокая скорость.</summary>
    [JsonStringEnumMemberName("high")] High,
    /// <summary>Нагрев.</summary>
    [JsonStringEnumMemberName("heat")] Heat,
    /// <summary>Охлаждение.</summary>
    [JsonStringEnumMemberName("cool")] Cool,
    /// <summary>Осушение.</summary>
    [JsonStringEnumMemberName("dry")] Dry,
    /// <summary>Только вентиляция.</summary>
    [JsonStringEnumMemberName("fan_only")] FanOnly,
}
