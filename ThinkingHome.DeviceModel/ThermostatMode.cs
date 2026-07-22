using System.Text.Json.Serialization;

namespace ThinkingHome.DeviceModel;

/// <summary>
/// Режим работы термостата/кондиционера — выбор из набора. Совпадает с Alice mode:thermostat
/// (auto/heat/cool/dry/fan_only) и с Matter Thermostat SystemMode. Устройство объявляет
/// поддержанное подмножество.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ThermostatMode
{
    Auto,
    Heat,
    Cool,
    Dry,
    FanOnly,
}
