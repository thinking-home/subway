using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Toggle;

/// <summary>Параметры toggle в discovery: переключаемая функция.</summary>
public class CapabilityToggleParams
{
    /// <summary>Переключаемая функция.</summary>
    [JsonPropertyName("instance")] public CapabilityToggleInstance Instance { get; set; }
}
