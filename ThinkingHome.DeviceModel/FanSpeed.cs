using System.Text.Json.Serialization;

namespace ThinkingHome.DeviceModel;

/// <summary>
/// Скорость вентиляции — выбор из набора. Совпадает с Alice mode:fan_speed (auto/low/medium/high)
/// и с Matter Fan Control FanMode. Устройство объявляет поддержанное подмножество.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FanSpeed
{
    Auto,
    Low,
    Medium,
    High,
}
