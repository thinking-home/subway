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
    /// <summary>Автоматический режим (устройство само выбирает обогрев/охлаждение).</summary>
    Auto,
    /// <summary>Обогрев.</summary>
    Heat,
    /// <summary>Охлаждение.</summary>
    Cool,
    /// <summary>Осушение.</summary>
    Dry,
    /// <summary>Только вентиляция (без обогрева и охлаждения).</summary>
    FanOnly,
}
