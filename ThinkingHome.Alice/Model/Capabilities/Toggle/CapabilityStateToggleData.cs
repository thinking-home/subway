using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Toggle;

/// <summary>Состояние toggle: инстанс и логическое значение.</summary>
public class CapabilityStateToggleData
{
    /// <summary>Переключаемая функция.</summary>
    [JsonPropertyName("instance")] public CapabilityToggleInstance Instance { get; set; }

    /// <summary>Включена ли функция.</summary>
    [JsonPropertyName("value")] public bool Value { get; set; }
}
