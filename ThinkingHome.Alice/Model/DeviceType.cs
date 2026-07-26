using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model;

/// <summary>Тип устройства в терминах Алисы (devices.types.*).</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DeviceType
{
    /// <summary>Осветительный прибор.</summary>
    [JsonStringEnumMemberName("devices.types.light")]
    Light,

    /// <summary>Розетка.</summary>
    [JsonStringEnumMemberName("devices.types.socket")]
    Socket,

    /// <summary>Выключатель.</summary>
    [JsonStringEnumMemberName("devices.types.switch")]
    Switch,

    /// <summary>Штора (devices.types.openable.curtain).</summary>
    [JsonStringEnumMemberName("devices.types.openable.curtain")]
    Curtain,

    /// <summary>Вентилятор (devices.types.ventilation.fan).</summary>
    [JsonStringEnumMemberName("devices.types.ventilation.fan")]
    Fan,

    /// <summary>Кондиционер (devices.types.thermostat.ac).</summary>
    [JsonStringEnumMemberName("devices.types.thermostat.ac")]
    ThermostatAc,

    /// <summary>Климатический датчик: температура, влажность, давление, CO2.</summary>
    [JsonStringEnumMemberName("devices.types.sensor.climate")]
    SensorClimate,

    /// <summary>Датчик движения.</summary>
    [JsonStringEnumMemberName("devices.types.sensor.motion")]
    SensorMotion,

    /// <summary>Датчик открытия двери/окна.</summary>
    [JsonStringEnumMemberName("devices.types.sensor.open")]
    SensorOpen,

    /// <summary>Датчик протечки воды.</summary>
    [JsonStringEnumMemberName("devices.types.sensor.water_leak")]
    SensorWaterLeak,

    /// <summary>Датчик освещённости.</summary>
    [JsonStringEnumMemberName("devices.types.sensor.illumination")]
    SensorIllumination,

    /// <summary>Устройство, не подходящее ни под один специальный тип.</summary>
    [JsonStringEnumMemberName("devices.types.other")]
    Other,

    /// <summary>Счётчик холодной воды (devices.types.smart_meter.cold_water).</summary>
    [JsonStringEnumMemberName("devices.types.smart_meter.cold_water")]
    SmartMeterColdWater,
}