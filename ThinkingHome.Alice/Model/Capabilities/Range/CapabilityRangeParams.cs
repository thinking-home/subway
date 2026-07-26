using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Range;

/// <summary>Параметры range в discovery: инстанс, единица измерения и границы диапазона.</summary>
public class CapabilityRangeParams
{
    /// <summary>Управляемый числовой параметр.</summary>
    [JsonPropertyName("instance")] public CapabilityStateRangeInstance Instance { get; set; }

    /// <summary>Единица измерения (константа из <see cref="Units"/>).</summary>
    [JsonPropertyName("unit")] public string Unit { get; set; }

    /// <summary>Доступна ли установка произвольного значения диапазона, а не только относительные шаги.</summary>
    [JsonPropertyName("random_access")] public bool RandomAccess { get; set; }

    /// <summary>Границы и шаг диапазона.</summary>
    [JsonPropertyName("range")] public CapabilityRangeLimits Range { get; set; }
}

/// <summary>Границы диапазона: минимум, максимум и шаг.</summary>
public class CapabilityRangeLimits
{
    /// <summary>Минимальное значение.</summary>
    [JsonPropertyName("min")] public float Min { get; set; }
    /// <summary>Максимальное значение.</summary>
    [JsonPropertyName("max")] public float Max { get; set; }
    /// <summary>Шаг изменения значения.</summary>
    [JsonPropertyName("precision")] public float Precision { get; set; }
}
