using ThinkingHome.DeviceModel.FluentApi.Capabilities;
using ThinkingHome.DeviceModel.FluentApi.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi;

/// <summary>
/// Хендл устройства. Аксессоры способностей/свойств без указания endpoint'а — синонимы
/// <c>Endpoint(0)</c> (0 — основной endpoint, как в ядре); никакого поиска «подходящего»
/// endpoint'а нет. «Нет устройства» — исключение из ядра на рабочих вызовах,
/// «нет способности» — данные (<see cref="CommandOutcome.Unsupported"/> / null).
/// </summary>
public readonly struct DeviceHandle
{
    private readonly IDeviceHost host;

    internal DeviceHandle(IDeviceHost host, string deviceId)
    {
        this.host = host;
        Id = deviceId;
    }

    /// <summary>Идентификатор устройства.</summary>
    public string Id { get; }

    /// <summary>Хендл endpoint'а (0 — основной).</summary>
    public EndpointHandle Endpoint(int endpointId) => new(host, Id, endpointId);

    /// <summary>Discovery: описание устройства или null, если его нет.</summary>
    public Task<DeviceDescriptor?> DescribeAsync(CancellationToken ct = default) => host.GetDeviceAsync(Id, ct);

    /// <summary>Полный снапшот состояния — как <see cref="IDeviceHost.QueryAsync"/> ядра.</summary>
    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default) => host.QueryAsync(Id, ct);

    /// <summary>Подписка на изменения этого устройства; отписка — Dispose.</summary>
    public IDisposable OnChanged(Action<StateChange> handler)
    {
        var id = Id; // лямбда в структуре не может захватить this
        return new ChangedSubscription(host, change =>
        {
            if (change.DeviceId == id) handler(change);
        });
    }

    /// <summary>Вкл/выкл (endpoint 0).</summary>
    public OnOffHandle OnOff() => Endpoint(0).OnOff();

    /// <summary>Яркость (endpoint 0).</summary>
    public BrightnessHandle Brightness() => Endpoint(0).Brightness();

    /// <summary>Цвет (endpoint 0).</summary>
    public ColorHandle Color() => Endpoint(0).Color();

    /// <summary>Степень открытия (endpoint 0).</summary>
    public OpenHandle Open() => Endpoint(0).Open();

    /// <summary>Скорость вентиляции (endpoint 0).</summary>
    public FanSpeedHandle FanSpeed() => Endpoint(0).FanSpeed();

    /// <summary>Осцилляция (endpoint 0).</summary>
    public OscillationHandle Oscillation() => Endpoint(0).Oscillation();

    /// <summary>Режим термостата (endpoint 0).</summary>
    public ThermostatModeHandle ThermostatMode() => Endpoint(0).ThermostatMode();

    /// <summary>Целевая температура (endpoint 0).</summary>
    public TargetTemperatureHandle TargetTemperature() => Endpoint(0).TargetTemperature();

    /// <summary>Температура (endpoint 0).</summary>
    public TemperatureHandle Temperature() => Endpoint(0).Temperature();

    /// <summary>Влажность (endpoint 0).</summary>
    public HumidityHandle Humidity() => Endpoint(0).Humidity();

    /// <summary>Атмосферное давление (endpoint 0).</summary>
    public PressureHandle Pressure() => Endpoint(0).Pressure();

    /// <summary>Освещённость (endpoint 0).</summary>
    public IlluminanceHandle Illuminance() => Endpoint(0).Illuminance();

    /// <summary>Присутствие/движение (endpoint 0).</summary>
    public OccupancyHandle Occupancy() => Endpoint(0).Occupancy();

    /// <summary>Контакт датчика открытия (endpoint 0).</summary>
    public ContactHandle Contact() => Endpoint(0).Contact();

    /// <summary>Протечка воды (endpoint 0).</summary>
    public WaterLeakHandle WaterLeak() => Endpoint(0).WaterLeak();

    /// <summary>Уровень заряда батареи (endpoint 0).</summary>
    public BatteryHandle Battery() => Endpoint(0).Battery();

    /// <summary>Индекс качества воздуха (endpoint 0).</summary>
    public AirQualityHandle AirQuality() => Endpoint(0).AirQuality();

    /// <summary>Концентрация CO2 (endpoint 0).</summary>
    public CarbonDioxideHandle CarbonDioxide() => Endpoint(0).CarbonDioxide();

    /// <summary>Показания счётчика воды (endpoint 0).</summary>
    public WaterMeterHandle WaterMeter() => Endpoint(0).WaterMeter();
}
