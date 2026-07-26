using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Mode;

/// <summary>Параметры mode в discovery: инстанс и список поддерживаемых режимов.</summary>
public class CapabilityModeParams
{
    /// <summary>Управляемая режимная функция.</summary>
    [JsonPropertyName("instance")] public CapabilityModeInstance Instance { get; set; }

    /// <summary>Поддерживаемые режимы.</summary>
    [JsonPropertyName("modes")] public CapabilityModeOption[] Modes { get; set; }
}

/// <summary>Один поддерживаемый режим в параметрах mode.</summary>
public class CapabilityModeOption
{
    /// <summary>Значение режима из словаря Алисы.</summary>
    [JsonPropertyName("value")] public CapabilityModeValue Value { get; set; }
}
