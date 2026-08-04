using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Drivers.Stubs;

/// <summary>Заглушка датчика качества воздуха (индекс качества + CO2 + заряд батареи, только чтение).</summary>
public sealed class StubAirQualitySensor(string id, StubDeviceConfig config) : IDevice
{
    private readonly AirQuality airQuality = AirQuality.Fair;
    private readonly double co2Ppm = 612;
    private readonly double battery = 91;

    /// <inheritdoc />
    public string Id => id;

    // статический стаб: изменений не эмитит
    /// <inheritdoc />
    public event Action<StateChange>? Changed { add { } remove { } }

    /// <inheritdoc />
    public DeviceDescriptor Describe() => new()
    {
        Id = id,
        Title = config.Title,
        Room = config.Room,
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub-aqs" },
        Endpoints = [new Endpoint
        {
            Id = 0,
            Type = DeviceType.AirQualitySensor,
            Properties =
            [
                new AirQualityProperty { Instance = AirQualityProperty.InstanceName },
                new CarbonDioxideProperty { Instance = CarbonDioxideProperty.InstanceName },
                new BatteryProperty { Instance = BatteryProperty.InstanceName },
            ],
        }],
    };

    /// <inheritdoc />
    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values =
            [
                new AirQualityState { Instance = AirQualityProperty.InstanceName, Value = airQuality },
                new CarbonDioxideState { Instance = CarbonDioxideProperty.InstanceName, Value = co2Ppm },
                new BatteryState { Instance = BatteryProperty.InstanceName, Value = battery },
            ],
        });

    // сенсор: команд нет
    /// <inheritdoc />
    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
        => Task.FromResult(CommandOutcome.Unsupported);
}
