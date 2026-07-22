namespace ThinkingHome.DeviceModel.Capabilities;

/// <summary>Режим работы термостата/кондиционера (выбор из набора). Matter cluster Thermostat (0x0201), атрибут SystemMode. Instance — "thermostat_mode".</summary>
public sealed record ThermostatModeCapability : Capability
{
    /// <summary>Поддерживаемые устройством режимы.</summary>
    public required IReadOnlyList<ThermostatMode> Modes { get; init; }
}
