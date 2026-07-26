using ThinkingHome.DeviceModel.FluentApi.Capabilities;
using ThinkingHome.DeviceModel.FluentApi.Properties;

namespace ThinkingHome.DeviceModel.FluentApi;

/// <summary>
/// Хендл endpoint'а устройства — адрес без I/O. Аксессоры возвращают хендлы способностей/свойств
/// с зашитым каноническим instance; наличие способности у устройства проверяется на вызовах
/// (или заранее — через <see cref="DescribeAsync"/> хендла).
/// </summary>
public readonly struct EndpointHandle
{
    private readonly IDeviceHost host;

    internal EndpointHandle(IDeviceHost host, string deviceId, int endpointId)
    {
        this.host = host;
        DeviceId = deviceId;
        EndpointId = endpointId;
    }

    /// <summary>Идентификатор устройства.</summary>
    public string DeviceId { get; }

    /// <summary>Номер endpoint'а внутри устройства (0 — основной).</summary>
    public int EndpointId { get; }

    /// <summary>Discovery: описание endpoint'а или null, если endpoint'а (или устройства) нет.</summary>
    public Task<Endpoint?> DescribeAsync(CancellationToken ct = default)
        => host.GetEndpointAsync(DeviceId, EndpointId, ct);

    /// <summary>Способность вкл/выкл.</summary>
    public OnOffHandle OnOff() => new(host, DeviceId, EndpointId);

    /// <summary>Способность «яркость», 0–100 %.</summary>
    public BrightnessHandle Brightness() => new(host, DeviceId, EndpointId);

    /// <summary>Способность «цвет»: RGB и/или цветовая температура.</summary>
    public ColorHandle Color() => new(host, DeviceId, EndpointId);

    /// <summary>Способность «степень открытия», 0–100 %.</summary>
    public OpenHandle Open() => new(host, DeviceId, EndpointId);

    /// <summary>Способность «скорость вентиляции».</summary>
    public FanSpeedHandle FanSpeed() => new(host, DeviceId, EndpointId);

    /// <summary>Способность «осцилляция» (поворот корпуса).</summary>
    public OscillationHandle Oscillation() => new(host, DeviceId, EndpointId);

    /// <summary>Способность «режим термостата».</summary>
    public ThermostatModeHandle ThermostatMode() => new(host, DeviceId, EndpointId);

    /// <summary>Способность «целевая температура» (уставка).</summary>
    public TargetTemperatureHandle TargetTemperature() => new(host, DeviceId, EndpointId);

    /// <summary>Свойство «температура», °C.</summary>
    public TemperatureHandle Temperature() => new(host, DeviceId, EndpointId);

    /// <summary>Свойство «относительная влажность», %.</summary>
    public HumidityHandle Humidity() => new(host, DeviceId, EndpointId);

    /// <summary>Свойство «атмосферное давление», кПа.</summary>
    public PressureHandle Pressure() => new(host, DeviceId, EndpointId);

    /// <summary>Свойство «освещённость», лк.</summary>
    public IlluminanceHandle Illuminance() => new(host, DeviceId, EndpointId);

    /// <summary>Свойство «присутствие/движение».</summary>
    public OccupancyHandle Occupancy() => new(host, DeviceId, EndpointId);

    /// <summary>Свойство «контакт датчика открытия».</summary>
    public ContactHandle Contact() => new(host, DeviceId, EndpointId);

    /// <summary>Свойство «протечка воды».</summary>
    public WaterLeakHandle WaterLeak() => new(host, DeviceId, EndpointId);

    /// <summary>Свойство «уровень заряда батареи», %.</summary>
    public BatteryHandle Battery() => new(host, DeviceId, EndpointId);

    /// <summary>Свойство «индекс качества воздуха».</summary>
    public AirQualityHandle AirQuality() => new(host, DeviceId, EndpointId);

    /// <summary>Свойство «концентрация CO2», ppm.</summary>
    public CarbonDioxideHandle CarbonDioxide() => new(host, DeviceId, EndpointId);

    /// <summary>Свойство «показания счётчика воды», м³.</summary>
    public WaterMeterHandle WaterMeter() => new(host, DeviceId, EndpointId);
}
