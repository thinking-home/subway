using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.Home;

/// <summary>
/// Заглушка климатической станции (температура + влажность + давление + заряд батареи, только чтение).
/// Составное устройство по правилу композиции Matter: термометр, гигрометр и барометр — независимые
/// прикладные роли, поэтому это три endpoint'а, а не несколько типов (или безбилетные кластеры) на одном.
/// </summary>
public sealed class StubClimateSensor(string id, string title, string? room = null) : IDevice
{
    private readonly double temperature = 23.5;
    private readonly double humidity = 41;
    private readonly double pressureKpa = 99.6;
    private readonly double battery = 87;

    public string Id => id;

    // статический стаб: изменений не эмитит
    public event Action<StateChange>? Changed { add { } remove { } }

    public DeviceDescriptor Describe() => new()
    {
        Id = id,
        Title = title,
        Room = room,
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub-climate" },
        Endpoints =
        [
            new Endpoint
            {
                Id = 0,
                Type = DeviceType.TemperatureSensor,
                Properties =
                [
                    new TemperatureProperty { Instance = "temperature" },
                    new BatteryProperty { Instance = "battery" },
                ],
            },
            new Endpoint
            {
                Id = 1,
                Type = DeviceType.HumiditySensor,
                Properties = [new HumidityProperty { Instance = "humidity" }],
            },
            new Endpoint
            {
                Id = 2,
                Type = DeviceType.PressureSensor,
                Properties = [new PressureProperty { Instance = "pressure" }],
            },
        ],
    };

    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values =
            [
                new TemperatureState { EndpointId = 0, Instance = "temperature", Value = temperature },
                new BatteryState { EndpointId = 0, Instance = "battery", Value = battery },
                new HumidityState { EndpointId = 1, Instance = "humidity", Value = humidity },
                new PressureState { EndpointId = 2, Instance = "pressure", Value = pressureKpa },
            ],
        });

    // сенсор: команд нет
    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
        => Task.FromResult(CommandOutcome.Unsupported);
}
