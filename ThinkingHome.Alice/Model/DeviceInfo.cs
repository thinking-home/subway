using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model;

/// <summary>Сведения об устройстве для discovery: производитель, модель, версии.</summary>
public class DeviceInfo
{
    /// <summary>Производитель устройства.</summary>
    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; set; }

    /// <summary>Модель устройства.</summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }

    /// <summary>Версия аппаратной части.</summary>
    [JsonPropertyName("hw_version")]
    public string HardwareVersion { get; set; }

    /// <summary>Версия программного обеспечения.</summary>
    [JsonPropertyName("sw_version")]
    public string SoftwareVersion { get; set; }
}