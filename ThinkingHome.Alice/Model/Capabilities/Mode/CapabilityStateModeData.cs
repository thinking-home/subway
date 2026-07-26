using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Mode;

/// <summary>Состояние mode: инстанс и текущий режим.</summary>
public class CapabilityStateModeData
{
    /// <summary>Управляемая режимная функция.</summary>
    [JsonPropertyName("instance")] public CapabilityModeInstance Instance { get; set; }

    /// <summary>Текущее значение режима.</summary>
    [JsonPropertyName("value")] public CapabilityModeValue Value { get; set; }
}
