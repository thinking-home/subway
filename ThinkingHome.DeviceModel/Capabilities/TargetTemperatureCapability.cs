namespace ThinkingHome.DeviceModel.Capabilities;

/// <summary>
/// Целевая температура (уставка термостата), °C — нормализованная единица ядра. Matter cluster
/// Thermostat (0x0201), атрибуты Occupied{Heating,Cooling}Setpoint. Instance — "target_temperature"
/// (не "temperature": тот занят сенсорным свойством, а кэш хоста ключуется (endpoint, instance)).
/// </summary>
public sealed record TargetTemperatureCapability : Capability
{
    /// <summary>Минимальная уставка, °C.</summary>
    public required int MinCelsius { get; init; }

    /// <summary>Максимальная уставка, °C.</summary>
    public required int MaxCelsius { get; init; }
}
