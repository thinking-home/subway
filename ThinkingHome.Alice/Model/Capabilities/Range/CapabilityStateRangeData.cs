using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Range;

/// <summary>Состояние range: инстанс и числовое значение.</summary>
public class CapabilityStateRangeData
{
    /// <summary>Управляемый числовой параметр.</summary>
    [JsonPropertyName("instance")] public CapabilityStateRangeInstance Instance { get; set; }

    /// <summary>Текущее значение.</summary>
    [JsonPropertyName("value")] public float Value { get; set; }
}
