using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.Home;

/// <summary>Заглушка датчика качества воздуха (индекс качества + CO2 + заряд батареи, только чтение).</summary>
public sealed class StubAirQualitySensor(string id, string title, string? room = null) : IDevice
{
    private readonly AirQuality airQuality = AirQuality.Fair;
    private readonly double co2Ppm = 612;
    private readonly double battery = 91;

    public string Id => id;

    // статический стаб: изменений не эмитит
    public event Action<StateChange>? Changed { add { } remove { } }

    public DeviceDescriptor Describe() => new()
    {
        Id = id,
        Title = title,
        Room = room,
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub-aqs" },
        Endpoints = [new Endpoint
        {
            Id = 0,
            Type = DeviceType.AirQualitySensor,
            Properties =
            [
                new AirQualityProperty { Instance = "air_quality" },
                new CarbonDioxideProperty { Instance = "carbon_dioxide" },
                new BatteryProperty { Instance = "battery" },
            ],
        }],
    };

    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values =
            [
                new AirQualityState { Instance = "air_quality", Value = airQuality },
                new CarbonDioxideState { Instance = "carbon_dioxide", Value = co2Ppm },
                new BatteryState { Instance = "battery", Value = battery },
            ],
        });

    // сенсор: команд нет
    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
        => Task.FromResult(CommandOutcome.Unsupported);
}
