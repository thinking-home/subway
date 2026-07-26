using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Range;

/// <summary>Инстанс способности range — какой числовой параметр устройства управляется.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CapabilityStateRangeInstance
{
    /// <summary>Яркость, %.</summary>
    [JsonStringEnumMemberName("brightness")] Brightness,
    /// <summary>Степень открытия, %.</summary>
    [JsonStringEnumMemberName("open")] Open,
    /// <summary>Целевая температура.</summary>
    [JsonStringEnumMemberName("temperature")] Temperature,
}
